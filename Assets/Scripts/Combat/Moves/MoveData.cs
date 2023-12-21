using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Moves/MoveData")]
public class MoveData : ScriptableObject
{
    public string id;
    public string moveName = "";
    [TextArea] public string description;
    public int power;
    public float hitChance = 1f;
    public bool alwaysHit = false;
    public int uses = 35;
    public TypeData moveType;
    public MoveCategoryId categoryId;
    public MoveTarget targetType;
    public List<MoveStatusChance> statusChances = new List<MoveStatusChance>();
    public List<MoveFieldChance> fieldChances = new List<MoveFieldChance>();
    public List<MoveStatChange> moveStatChanges = new List<MoveStatChange>();
    public List<ConditionalMoveBonus> conditionalBonuses = new List<ConditionalMoveBonus>();
    public bool isContact;
    public float drainMultiplier = 0f;
    public int moveCritUp = 0;
    public int priority = 0;
    public float percentageHealthCost = 0f;
    public bool stealInsteadOfDestroy = false;
    public List<ItemCategory> destroyHeldItems = new List<ItemCategory>();
    // If you want the sound to started delayed you need to create a prefab that plays the sound and put it inside the animations list in the moment you want
    public AudioOptions soundEffect;
    public List<BattleAnimationPokemon> animations = new List<BattleAnimationPokemon>();

    public string GetId()
    {
        return id;
    }

    // This will be Executed after a pokemon Move Event
    public virtual void Execute(BattleEventUseMove battleEvent)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        battleEvent.pokemon.ReduceMovePP(this);
        PokemonBattleData pokemonTarget = bm.GetTarget(battleEvent.pokemon, battleEvent.move.targetType);
        bool moveHits = bm.CheckForMoveHit(battleEvent) || alwaysHit;
        bool hasEnoughHealth = battleEvent.pokemon.GetHealthPercentage() >= percentageHealthCost;
        if (!hasEnoughHealth)
        {
            moveHits = false;
        }
        // Move use anim
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPokemonMoveFlowchart(battleEvent));
        if (!moveHits)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventMoveMiss(battleEvent));
            BattleMaster.GetInstance().GetCurrentBattle().AddEvent(new BattleEventUseMoveFail(battleEvent));
        }
        else
        {
            if (categoryId != MoveCategoryId.status)
            {
                DamageSummary damageSummary = GetMoveDamageSummary(battleEvent);
                // If the opponent isn't inmune then we deal damage, show anims, etc
                if (damageSummary.advantageType != BattleTypeAdvantageType.inmune)
                {
                    bm.AddDamageDealtEvent(pokemonTarget, damageSummary);
                }
                else
                {
                    BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventTypeAdvantage(damageSummary.advantageType));
                }
            }
            else
            {
                HandleAnimations(battleEvent.pokemon, pokemonTarget);
                HandleStatsChanges(battleEvent.pokemon);
                HandleStatusAdd(battleEvent.pokemon);
                HandleFieldStatusAdd();
                HandleDestroy(pokemonTarget, battleEvent.pokemon);
            }
            bm.AddMoveSuccessEvent(battleEvent);
            // Negative values are used for recoil
            if (drainMultiplier != 0 && moveHits)
            {
                BattleMaster.GetInstance().GetCurrentBattle()?.AddTrigger(new BattleTriggerDrainOnMoveDamage(battleEvent.pokemon, this, drainMultiplier));
            }
            // Handle health pay
            if (hasEnoughHealth)
            {
                DamageSummary selfDamage = new DamageSummary(
                    TypesMaster.Instance.nullType,
                    (int)(battleEvent.pokemon.GetMaxHealth() * percentageHealthCost),
                    DamageSummarySource.MoveEffect,
                    GetId()
                );
                bm.AddDamageDealtEvent(battleEvent.pokemon, selfDamage);
            }
        }
    }

    public virtual DamageSummary GetMoveDamageSummary(BattleEventUseMove battleEvent)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        DamageSummary damageSummary = bm.CalculateMoveDamage(battleEvent);
        return damageSummary;
    }

    public virtual void HandleStatsChanges(PokemonBattleData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        foreach(MoveStatChange msc in moveStatChanges)
        {
            PokemonBattleData pokemonTarget = bm.GetTarget(pokemon, msc.targetType);
            float random = Random.value;
            if (random < msc.changeChance)
            {
                bm.AddStatChangeEvent(pokemonTarget, msc.statsAmountChange);
            }
        }
    }

    public virtual void HandleStatusAdd(PokemonBattleData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        foreach (MoveStatusChance msc in statusChances)
        {
            PokemonBattleData pokemonTarget = bm.GetTarget(pokemon, msc.targetType);
            float random = Random.value;
            if (random < msc.chance)
            {
                bm.AddStatusEffectEvent(pokemonTarget, msc.effectData, this, GetAttackCategory() == MoveCategoryId.status);
            }
        }
    }

    public virtual void HandleFieldStatusAdd()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        foreach (MoveFieldChance mfc in fieldChances)
        {
            float random = Random.value;
            if (random < mfc.chance)
            {
                if (mfc.removeStatusInstead)
                {
                    bm.RemoveStatusField(mfc.status);
                }
                else
                {
                    bm.AddStatusFieldEvent(mfc.status);
                }
            }
        }
    }

    public void HandleAnimations(PokemonBattleData user, PokemonBattleData target)
    {
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPlaySound(soundEffect));
        foreach(BattleAnimationPokemon anim in animations)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(
                new BattleAnimatorEventAnimation(user, target, anim)
            );
        }
    }

    public void HandleDestroy(PokemonBattleData target, PokemonBattleData user)
    {
        PokemonBattleDataItem item = target.heldItem;
        if (item != null && item.equippedItem && destroyHeldItems.Contains(item.equippedItem.GetItemCategory()))
        {
            if (stealInsteadOfDestroy)
            {
                if (user.heldItem != null)
                {
                    user.EquipItem(item.equippedItem);
                    target.UnequipItem();
                }
            }
            else
            {
                target.UnequipItem();
            }
        }
    }

    // Use this in combat to get final power
    public virtual int GetPower(PokemonBattleData user)
    {
        float powerMultiplier = 1f;
        foreach(ConditionalMoveBonus condition in conditionalBonuses)
        {
            PokemonBattleData finalTarget = BattleMaster.GetInstance().GetCurrentBattle().GetTarget(user, condition.target);
            if (condition.MeetsConditions(finalTarget, this))
            {
                powerMultiplier *= condition.powerMultiplier;
            }
        }
        return (int)(power * powerMultiplier);
    }

    public virtual int GetPower()
    {
        return power;
    }

    public virtual float GetAccuracy(PokemonBattleData user)
    {
        float accuracyAdded = 0f;
        foreach (ConditionalMoveBonus condition in conditionalBonuses)
        {
            PokemonBattleData finalTarget = BattleMaster.GetInstance().GetCurrentBattle().GetTarget(user, condition.target);
            if (condition.MeetsConditions(finalTarget, this))
            {
                accuracyAdded += condition.accuracyBonusAdd;
            }
        }
        return hitChance + accuracyAdded;
    }

    public virtual int GetCritLevel()
    {
        return moveCritUp;
    }

    public virtual MoveCategoryId GetDefenseCategory()
    {
        return categoryId;
    }

    public virtual MoveCategoryId GetAttackCategory()
    {
        return categoryId;
    }
    public virtual TypeData GetMoveType()
    {
        return moveType;
    }

    public virtual int GetPriority(BattleManager battleStatus, BattleTeamId myTeam)
    {
        PokemonBattleData myUser = battleStatus.GetTeamActivePokemon(myTeam);
        PokemonBattleData myEnemy = battleStatus.GetTeamActivePokemon(myTeam == BattleTeamId.Team1 ? BattleTeamId.Team2 : BattleTeamId.Team1);
        int myRoundsInCombat = myUser.roundsInCombat;
        float myHealthPercentage = (float)myUser.GetPokemonCurrentHealth() / myUser.GetMaxHealth();
        bool amILossingOnSpeed = priority <= 0 && myUser.GetBattleStats().speed < myEnemy.GetBattleStats().speed;
        if (categoryId == MoveCategoryId.status)
        {
            float alreadyHasStatusBiasMultiplier = 1f;
            StatusEffect enemyStatusPrimary = myEnemy.GetCurrentPrimaryStatus();
            List<StatusEffect> enemyMinorStatuses = myEnemy.GetNonPrimaryStatus();
            float lossingOnSpeedPow = amILossingOnSpeed ? 1.4f : 1f;
            if (statusChances.Count > 0)
            {
                foreach(MoveStatusChance statusChance in statusChances)
                {
                    if (enemyStatusPrimary != null && enemyStatusPrimary.effectData.GetId() == statusChance.effectData.GetId())
                    {
                        alreadyHasStatusBiasMultiplier = 0f;
                        break;
                    }
                    foreach(StatusEffect se in enemyMinorStatuses)
                    {
                        if (se.effectData.GetId() == statusChance.effectData.GetId())
                        {
                            alreadyHasStatusBiasMultiplier = 0f;
                            break;
                        }
                    }
                    PokemonBattleData target = battleStatus.GetTarget(myUser, statusChance.targetType);
                    StatusEffectData stat = statusChance.effectData;
                    if (UtilsMaster.ContainsAtLeastOne(stat.GetInmuneTypes(), target.GetTypes()))
                    {
                        alreadyHasStatusBiasMultiplier = 0f;
                    }
                }
            }
            return (int)Mathf.Ceil((8f - 1 * myRoundsInCombat) * Mathf.Pow(myHealthPercentage, lossingOnSpeedPow) * alreadyHasStatusBiasMultiplier);
        }
        else
        {
            DamageSummary possibleDamage = battleStatus.CalculateMoveDamage(new BattleEventUseMove(myUser, this));
            float percentageDamage = (float)possibleDamage.damageAmount / myUser.GetMaxHealth();
            bool willKill = possibleDamage.damageAmount >= myUser.GetPokemonCurrentHealth();
            return willKill && !amILossingOnSpeed ? 11 : (int)Mathf.Ceil((10f * (0.2f + 0.8f * percentageDamage)));
        }
    }
}

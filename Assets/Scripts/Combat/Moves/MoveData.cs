using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Moves/MoveData")]
public class MoveData : ScriptableObject
{
    public MoveId moveId;
    public string moveName = "";
    public int power;
    public float hitChance = 1f;
    public bool alwaysHit = false;
    public int uses = 35;
    public PokemonTypeId typeId;
    public MoveCategoryId categoryId;
    public MoveTarget targetType;
    public List<MoveStatusChance> statusChances = new List<MoveStatusChance>();
    public List<MoveStatChange> moveStatChanges = new List<MoveStatChange>();
    public List<StatusBonus> conditionalBonuses = new List<StatusBonus>();
    public List<MoveTags> tags = new List<MoveTags>();
    public bool isContact;
    public float drainMultiplier = 0f;
    public int moveCritUp = 0;
    public int priority = 0;
    public bool stealInsteadOfDestroy = false;
    public List<ItemCategory> destroyHeldItems = new List<ItemCategory>();
    [TextArea]
    public string description;
    public List<BattleAnimation> animations = new List<BattleAnimation>();

    // This will be Executed after a pokemon Move Event
    public virtual void Execute(BattleEventUseMove battleEvent)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        battleEvent.pokemon.ReduceMovePP(this);
        PokemonBattleData pokemonTarget = bm.GetTarget(battleEvent.pokemon, battleEvent.move.targetType);
        bool moveHits = bm.CheckForMoveHit(battleEvent) || alwaysHit;
        // Move use anim
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPokemonMove(battleEvent));
        if (!moveHits)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventMoveMiss(battleEvent));
        }
        else
        {
            if (categoryId != MoveCategoryId.status)
            {
                DamageSummary damageSummary = GetMoveDamageSummary(battleEvent);
                bm.AddDamageDealtEvent(pokemonTarget, damageSummary);
            }
            else
            {
                HandleAnimations(battleEvent.pokemon, pokemonTarget);
                HandleStatsChanges(battleEvent.pokemon);
                HandleStatusAdds(battleEvent.pokemon);
                HandleDestroy(pokemonTarget, battleEvent.pokemon);
            }
            bm.AddMoveSuccessEvent(battleEvent);
            // Negative values are used for recoil
            if (drainMultiplier != 0 && moveHits)
            {
                BattleMaster.GetInstance().GetCurrentBattle()?.AddTrigger(new BattleTriggerDrainOnMoveDamage(battleEvent.pokemon, this, drainMultiplier));
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

    public virtual void HandleStatusAdds(PokemonBattleData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        foreach (MoveStatusChance msc in statusChances)
        {
            PokemonBattleData pokemonTarget = bm.GetTarget(pokemon, msc.targetType);
            float random = Random.value;
            if (random < msc.chance)
            {
                bm.AddStatusEffectEvent(pokemonTarget,msc.effectId, categoryId == MoveCategoryId.status);
            }
        }
    }

    public void HandleAnimations(PokemonBattleData user, PokemonBattleData target)
    {
        foreach(BattleAnimation anim in animations)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(
                new BattleAnimatorEventPokemonMoveAnimation(user, target, anim)
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
                    user.EquippedItem(item.equippedItem);
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
        foreach(StatusBonus condition in conditionalBonuses)
        {
            float percentageHealth = user.GetPokemonCurrentHealth() / (float) user.GetMaxHealth();
            if (condition.target == MoveTarget.none)
            {
                Status weather = BattleMaster.GetInstance().GetCurrentBattle().weather;
                if (weather != null && weather.effectId == condition.statusToCheck && percentageHealth <= condition.lifeBelowTreshold)
                {
                    powerMultiplier *= condition.powerMultiplier;
                }
            }
            else
            {
                PokemonBattleData finalTarget = BattleMaster.GetInstance().GetCurrentBattle().GetTarget(user, condition.target);
                List<StatusEffect> status = finalTarget.GetNonPrimaryStatus();
                StatusEffect primary = finalTarget.GetCurrentPrimaryStatus();
                foreach (StatusEffect s in status)
                {
                    if (condition.statusToCheck == s.effectId || (primary != null && primary.effectId == condition.statusToCheck))
                    {
                        powerMultiplier *= condition.powerMultiplier;
                    }
                }
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
        foreach (StatusBonus condition in conditionalBonuses)
        {
            float percentageHealth = user.GetPokemonCurrentHealth() / (float)user.GetMaxHealth();
            if (condition.target == MoveTarget.none)
            {
                Status weather = BattleMaster.GetInstance().GetCurrentBattle().weather;
                if (weather != null && weather.effectId == condition.statusToCheck && percentageHealth <= condition.lifeBelowTreshold)
                {
                    accuracyAdded += condition.accuracyBonusAdd;
                }
            }
            else
            {
                PokemonBattleData finalTarget = BattleMaster.GetInstance().GetCurrentBattle().GetTarget(user, condition.target);
                List<StatusEffect> status = finalTarget.GetNonPrimaryStatus();
                StatusEffect primary = finalTarget.GetCurrentPrimaryStatus();
                foreach (StatusEffect s in status)
                {
                    if (condition.statusToCheck == s.effectId || (primary != null && primary.effectId == condition.statusToCheck))
                    {
                        accuracyAdded += condition.accuracyBonusAdd;
                    }
                }
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
                    if (enemyStatusPrimary != null && enemyStatusPrimary.effectId == statusChance.effectId)
                    {
                        alreadyHasStatusBiasMultiplier = 0f;
                        break;
                    }
                    foreach(StatusEffect se in enemyMinorStatuses)
                    {
                        if (se.effectId == statusChance.effectId)
                        {
                            alreadyHasStatusBiasMultiplier = 0f;
                            break;
                        }
                    }
                    PokemonBattleData target = battleStatus.GetTarget(myUser, statusChance.targetType);
                    StatusEffect stat = battleStatus.CreateStatusById(statusChance.effectId, target);
                    if (UtilsMaster.ContainsAtLeastOne(stat.inmuneTypes, target.GetTypeIds()))
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

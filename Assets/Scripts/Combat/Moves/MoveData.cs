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
    public bool isContact;
    public float drainMultiplier = 0f;
    public int moveCritUp = 0;
    public int priority = 0;
    public string description;
    public List<BattleAnimation> animations = new List<BattleAnimation>();

    // This will be Executed after a pokemon Move Event
    public virtual void Execute(BattleEventUseMove battleEvent)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        HandleStatsChanges(battleEvent.pokemon);
        HandleStatusAdds(battleEvent.pokemon);
        battleEvent.pokemon.ReduceMovePP(this);
        PokemonBattleData pokemonTarget = bm.GetTarget(battleEvent.pokemon, battleEvent.move.targetType);
        if (categoryId != MoveCategoryId.status)
        {
            DamageSummary damageSummary = bm.CalculateMoveDamage(battleEvent);
            bm.AddDamageDealtEvent(pokemonTarget, damageSummary);

        }
        bm.AddMoveSuccessEvent(battleEvent);
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPokemonMove(battleEvent));
        // Negative values are used for recoil
        if (drainMultiplier != 0)
        {
            BattleMaster.GetInstance().GetCurrentBattle()?.AddTrigger(new BattleTriggerDrainOnMoveDamage(battleEvent.pokemon, this, drainMultiplier));
        }
        HandleAnimations(battleEvent.pokemon, pokemonTarget);
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

    // Use this in combat to get final power
    public virtual int GetPower(PokemonBattleData user)
    {
        return power;
    }
    public virtual int GetPower()
    {
        return power;
    }

    public virtual MoveCategoryId GetDefenseCategory()
    {
        return categoryId;
    }

    public virtual MoveCategoryId GetAttackCategory()
    {
        return categoryId;
    }
}

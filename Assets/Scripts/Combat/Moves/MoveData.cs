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
    public bool drainsDamage;
    public string description;

    // This will be Executed after a pokemon Move Event
    public virtual void Execute(BattleEventUseMove battleEvent)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (categoryId != MoveCategoryId.status)
        {
            DamageSummary damageSummary = bm.CalculateMoveDamage(battleEvent);
            PokemonBattleData pokemonTarget = bm.GetTarget(battleEvent.pokemon, battleEvent.move.targetType);
            bm.AddDamageDealtEvent(pokemonTarget, damageSummary);
        }
        HandleStatsChanges(battleEvent.pokemon);
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPokemonMove(battleEvent));
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

    public virtual int GetPower(PokemonBattleData user)
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

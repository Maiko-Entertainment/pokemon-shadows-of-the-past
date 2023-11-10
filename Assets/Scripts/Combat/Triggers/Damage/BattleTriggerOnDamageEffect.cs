using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnDamageEffect : BattleTriggerOnPokemonDamage
{
    public List<DamageSummarySource> damageSources = new List<DamageSummarySource>() { DamageSummarySource.Move };
    public bool requiresContact = true; // Only used for moves
    public bool isAbilitySource = false;
    public bool workOnlyOnOppositeGender = false;
    public List<MoveStatusChance> statusChances = new List<MoveStatusChance>();
    public List<MoveStatChange> moveStatChanges = new List<MoveStatChange>();
    public List<PokemonTypeId> affectedMoveTypes = new List<PokemonTypeId>();

    public BattleTriggerOnDamageEffect(PokemonBattleData pokemon, List<MoveStatusChance> statusChances, List<MoveStatChange> moveStatChanges) : base(pokemon, true)
    {
        this.statusChances = statusChances;
        this.moveStatChanges = moveStatChanges;
        maxTriggers = 999999;
    }

    public override bool Execute(BattleEventTakeDamageSuccess battleEvent)
    {
        DamageSummary summary = battleEvent.damageEvent.damageSummary;
        bool wasTriggered = false;
        if (damageSources.Contains(summary.damageSource))
        {
            if (!IsRestrictedByGender(battleEvent))
            {
                // Checks for required contact requirement which applies only to moves
                if (summary.damageSource == DamageSummarySource.Move)
                {
                    MoveData moveData = MovesMaster.Instance.GetMove(summary.sourceId);
                    bool meetsMoveTypeCondition = true;
                    if (affectedMoveTypes.Count > 0 && !affectedMoveTypes.Contains(moveData.GetMoveType()))
                    {
                        meetsMoveTypeCondition = false;
                    }
                    // If requires contact but it doesnt have contact exits
                    if (requiresContact && !moveData.isContact || !meetsMoveTypeCondition)
                    {
                        return base.Execute(battleEvent);
                    }
                }
                wasTriggered = wasTriggered || HandleStatsChanges(pokemon);
                wasTriggered = wasTriggered || HandleStatusAdds(pokemon);
            }
            if (wasTriggered)
                BattleMaster.GetInstance().GetCurrentBattle().AddAbilityEvent(pokemon);
        }
        return base.Execute(battleEvent);
    }

    public bool IsRestrictedByGender(BattleEventTakeDamageSuccess battleEvent)
    {
        if (workOnlyOnOppositeGender)
        {
            DamageSummary summary = battleEvent.damageEvent.damageSummary;
            if (summary.pokemonSource != null)
            {
                PokemonCaughtData dealer = summary.pokemonSource.GetPokemonCaughtData();
                if (!dealer.GetPokemonBaseData().isGenderless && dealer.isMale == battleEvent.pokemon.GetPokemonCaughtData().isMale)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }

        }
        else
        {
            return false;
        }
    }

    public virtual bool HandleStatsChanges(PokemonBattleData pokemon)
    {
        bool wasTriggered = false;
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        foreach (MoveStatChange msc in moveStatChanges)
        {
            PokemonBattleData pokemonTarget = bm.GetTarget(pokemon, msc.targetType);
            float random = Random.value;
            if (random < msc.changeChance)
            {
                bm.AddStatChangeEvent(pokemonTarget, msc.statsAmountChange);
                wasTriggered = true;
            }
        }
        return wasTriggered;
    }

    public virtual bool HandleStatusAdds(PokemonBattleData pokemon)
    {
        bool wasTriggered = false;
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        foreach (MoveStatusChance msc in statusChances)
        {
            PokemonBattleData pokemonTarget = bm.GetTarget(pokemon, msc.targetType);
            float random = Random.value;
            bool hasStatus = pokemonTarget.GetNonPrimaryStatus().Find((se) => msc.effectId == se.effectId) != null;
            bool hasPrimaryAlready = pokemonTarget.GetCurrentPrimaryStatus() != null && pokemonTarget.GetCurrentPrimaryStatus().effectId == msc.effectId;
            if (random < msc.chance && !hasStatus && !hasPrimaryAlready)
            {
                bm.AddStatusEffectEvent(pokemonTarget, msc.effectId, isAbilitySource);
                wasTriggered = true;
            }
        }
        return wasTriggered;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonTakeMove : BattleTriggerOnPokemonMove
{
    public List<PokemonTypeId> affectedTypes = new List<PokemonTypeId>();
    public bool grantsInmunity = false;
    public bool showAbility = false;

    public List<MoveStatusChance> statusChances = new List<MoveStatusChance>();
    public List<MoveStatChange> statChanges = new List<MoveStatChange>();
    public BattleTriggerOnPokemonTakeMove(PokemonBattleData pokemon, UseMoveMods moveMods = null, bool showAbility = false):
        base(pokemon, moveMods, true)
    {
        this.showAbility = showAbility;
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {
        PokemonBattleData target = BattleMaster.GetInstance().GetCurrentBattle().GetTarget(battleEvent.pokemon, battleEvent.move.targetType);
        if (target.battleId == pokemon.battleId && maxTriggers > 0)
        {
            bool isApplicable = true;
            bool willTrigger = false;
            bool showedAbility = !showAbility;
            if (affectedTypes.Count > 0)
            {
                if (!affectedTypes.Contains(battleEvent.move.typeId)){
                    isApplicable = false;
                }
            }
            if (isApplicable)
            {
                // Only show if any stat or status chance was met
                willTrigger = willTrigger || HandleStatsChanges(pokemon);
                willTrigger = willTrigger || HandleStatusAdds(pokemon);
                if (useMoveMods != null)
                {
                    battleEvent.moveMods.Implement(useMoveMods);
                    willTrigger = true;
                }
                if (grantsInmunity)
                {
                    willTrigger = true;
                    BattleMaster.GetInstance()?.GetCurrentBattle()?.AddEvent(
                        new BattleEventNarrative(
                            new BattleTriggerMessageData(
                                BattleAnimatorMaster.GetInstance().battleFlowchart,
                                "Inmune"
                            )));
                    if (!showedAbility)
                    {
                        showedAbility = true;
                        BattleMaster.GetInstance().GetCurrentBattle().AddAbilityEvent(target);
                    }
                    BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPokemonMoveFlowchart(battleEvent));
                    maxTriggers--;
                    return false;
                }
                if (!showedAbility && willTrigger)
                    BattleMaster.GetInstance().GetCurrentBattle().AddAbilityEvent(target);
            }
            else
            {
                maxTriggers++;
            }
        }
        else
        {
            maxTriggers++;
        }
        return base.Execute(battleEvent);
    }
    public virtual bool HandleStatsChanges(PokemonBattleData pokemon)
    {
        bool wasTriggered = false;
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        foreach (MoveStatChange msc in statChanges)
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
                bm.AddStatusEffectEvent(pokemonTarget, msc.effectId, showAbility);
                wasTriggered = true;
            }
        }
        return wasTriggered;
    }
}

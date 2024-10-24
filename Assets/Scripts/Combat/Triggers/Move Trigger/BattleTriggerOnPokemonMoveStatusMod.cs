using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonMoveStatusMod : BattleTriggerOnPokemonMove
{
    public bool showAbility = false;
    public List<StatusEffectId> includedStatus = new List<StatusEffectId>();
    public BattleTriggerOnPokemonMoveStatusMod(PokemonBattleData pokemon, UseMoveMods moveMods, List<StatusEffectId> includedStatus, bool showAbility=false): base(pokemon, moveMods, true)
    {
        this.showAbility = showAbility;
        this.includedStatus = includedStatus;
    }
    public override bool Execute(BattleEventUseMove battleEvent)
    {
        if (pokemon.battleId == battleEvent.pokemon.battleId && maxTriggers > 0)
        {
            bool includesPrimary = pokemon.GetCurrentPrimaryStatus() != null ? includedStatus.Contains(pokemon.GetCurrentPrimaryStatus().effectId) : false;
            bool includesOthers = false;
            foreach(StatusEffect se in pokemon.GetNonPrimaryStatus())
            {
                if (includedStatus.Contains(se.effectId))
                {
                    includesOthers = true;
                    break;
                }
            }
            if (includesPrimary || includesOthers)
            {
                battleEvent.moveMods.Implement(useMoveMods);
                maxTriggers--;
                if (showAbility)
                {
                    BattleMaster.GetInstance().GetCurrentBattle()?.AddAbilityEvent(pokemon);
                }
                return true;
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
        return true;
    }
}

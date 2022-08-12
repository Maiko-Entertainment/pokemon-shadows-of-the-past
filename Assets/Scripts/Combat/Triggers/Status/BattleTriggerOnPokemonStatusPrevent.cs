using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonStatusPrevent : BattleTriggerOnPokemonGainStatusEffect
{
    public List<StatusEffectId> preventableStatuses = new List<StatusEffectId>();
    public bool showAbility;

    public BattleTriggerOnPokemonStatusPrevent(PokemonBattleData pokemon, List<StatusEffectId> preventableStatuses, bool showAbility = false):
        base(pokemon)
    {
        this.preventableStatuses = preventableStatuses;
        this.showAbility = showAbility;
    }

    public override bool Execute(BattleEventPokemonStatusAdd battleEvent)
    {
        if (
            preventableStatuses.Contains(battleEvent.statusId) &&
            pokemon.battleId == battleEvent.pokemon.battleId &&
            maxTriggers > 0
           )
        {
            if (showAbility)
            {
                BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorAbility(pokemon));
            }
            return false;
        }
        maxTriggers++;
        return base.Execute(battleEvent);
    }
}

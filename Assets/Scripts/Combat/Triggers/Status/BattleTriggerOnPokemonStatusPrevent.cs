using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonStatusPrevent : BattleTriggerOnPokemonGainStatusEffect
{
    public List<StatusEffectData> preventableStatuses = new List<StatusEffectData>();
    public bool showAbility;

    public BattleTriggerOnPokemonStatusPrevent(PokemonBattleData pokemon, List<StatusEffectData> preventableStatuses, bool showAbility = false):
        base(pokemon)
    {
        this.preventableStatuses = preventableStatuses;
        this.showAbility = showAbility;
    }

    public override bool Execute(BattleEventPokemonStatusAdd battleEvent)
    {
        if (
            preventableStatuses.Contains(battleEvent.status) &&
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

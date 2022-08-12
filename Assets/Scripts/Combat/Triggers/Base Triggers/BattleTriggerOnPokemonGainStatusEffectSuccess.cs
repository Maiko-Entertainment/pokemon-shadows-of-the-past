using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonGainStatusEffectSuccess : BattleTriggerOnPokemon
{
    public BattleTriggerOnPokemonGainStatusEffectSuccess(PokemonBattleData pokemon):
        base(pokemon, true)
    {
        eventId = BattleEventId.pokemonAddStatusSuccess;
    }

    public virtual bool Execute(BattleEventPokemonStatusAddSuccess battleEvent)
    {
        return base.Execute(battleEvent);
    }
}

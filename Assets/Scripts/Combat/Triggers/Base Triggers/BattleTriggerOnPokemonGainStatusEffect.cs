using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonGainStatusEffect : BattleTriggerOnPokemon
{
    public BattleTriggerOnPokemonGainStatusEffect(PokemonBattleData pokemon):
        base(pokemon, true)
    {
        eventId = BattleEventId.pokemonAddStatus;
    }

    public virtual bool Execute(BattleEventPokemonStatusAdd battleEvent)
    {
        return base.Execute(battleEvent);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventEnterPokemon : BattleEventPokemon
{
    public BattleEventEnterPokemon(PokemonBattleData pokemon) :
         base(pokemon)
    {
        eventId = BattleEventId.pokemonEnter;
    }
}

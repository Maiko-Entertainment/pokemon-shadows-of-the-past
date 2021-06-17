using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemon : BattleEvent
{
    public PokemonBattleData pokemon;

    public BattleEventPokemon (PokemonBattleData pokemon)
    {
        this.pokemon = pokemon;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventPokemon : BattleAnimatorEvent
{
    public PokemonBattleData pokemon = new PokemonBattleData();

    public BattleAnimatorEventPokemon(PokemonBattleData pokemon)
    {
        this.pokemon = pokemon;
    }
}

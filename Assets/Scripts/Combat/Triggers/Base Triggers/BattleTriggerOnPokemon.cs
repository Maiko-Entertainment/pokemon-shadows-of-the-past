using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemon : BattleTrigger
{
    public PokemonBattleData pokemon;

    public BattleTriggerOnPokemon(PokemonBattleData pokemon, bool deleteOnLeave)
    {
        this.pokemon = pokemon;
        if (deleteOnLeave)
        {
            BattleMaster.GetInstance().GetCurrentBattle()
            .AddTrigger(new BattleTriggerCleanUp(pokemon, this));
        }
    }

    public override string ToString()
    {
        return ""+pokemon.pokemon.pokemonName;
    }
}

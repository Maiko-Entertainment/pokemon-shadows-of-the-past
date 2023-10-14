using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEventStartEvolution : InteractionEvent
{
    PokemonCaughtData pokemon;
    PokemonBaseData evolution;
    public InteractionEventStartEvolution(PokemonCaughtData pokemon, PokemonBaseData evolution)
    {
        this.pokemon = pokemon;
        this.evolution = evolution;
    }

    public override void Execute()
    {
        PokemonMaster.GetInstance().EvolvePokemon(pokemon, evolution);
        InteractionsMaster.GetInstance().ExecuteNext();
    }
}

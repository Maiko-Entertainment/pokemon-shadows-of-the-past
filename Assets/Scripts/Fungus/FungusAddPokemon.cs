using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Game",
    "Add Pokemon to Party",
    "Adds a pokemon to the party."
)]
public class FungusAddPokemon : Command
{
    public PokemonCaughtData pokemonToAdd;
    public override void OnEnter()
    {
        PartyMaster.GetInstance().AddPartyMember(pokemonToAdd);
        Continue();
    }
}

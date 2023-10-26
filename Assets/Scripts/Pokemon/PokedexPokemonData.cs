using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokedexPokemonData
{
    public PokemonBaseData pokemon;
    public int caughtAmount = 0;
    public int seenAmount = 0;

    public PokedexPokemonData(PokemonBaseData pokemon)
    {
        this.pokemon = pokemon;
    }

    public PersistedPokedexPokemonData GetSave()
    {
        PersistedPokedexPokemonData saveData = new PersistedPokedexPokemonData();
        saveData.id = pokemon.GetId();
        saveData.caughtAmount = caughtAmount;
        saveData.seenAmount = seenAmount;
        return saveData;
    }
}

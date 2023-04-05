using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPokedexSectionBasicData : MonoBehaviour
{
    public TextMeshProUGUI pokedexEntry;
    public Transform evolutionList;
    public Transform foundInList;

    public UIPokedexEvolution evolutionPrefab;
    public UIPokedexFoundIn foundInPrefab;

    public UIPokedexSectionBasicData Load(PokedexPokemonData pokemonData)
    {
        PokemonBaseData pokemon = pokemonData.pokemon;
        if (pokemonData.caughtAmount > 0)
        {
            pokedexEntry.text = pokemon.pokedexEntry;
        }
        else
        {
            pokedexEntry.text = "Catch this pokemon to learn more about it.";
        }
        foreach (Transform evo in evolutionList)
        {
            Destroy(evo.gameObject);
        }
        foreach (Transform evo in foundInList)
        {
            Destroy(evo.gameObject);
        }
        foreach (PokemonBaseEvolution evo in pokemon.evolutions)
        {
            PokedexPokemonData evoBase = PokemonMaster.GetInstance().GetPokemonPokedexData(evo.pokemonId);
            Instantiate(evolutionPrefab, evolutionList).Load(evo, pokemonData.seenAmount > 0, evoBase.seenAmount > 0);
        }
        if (foundInPrefab)
        {
            foreach (PokemonBaseDataAreas data in pokemon.encounteredIn)
            {
                Instantiate(foundInPrefab, foundInList).Load(pokemonData, data);
            }
        }
        return this;
    }
}

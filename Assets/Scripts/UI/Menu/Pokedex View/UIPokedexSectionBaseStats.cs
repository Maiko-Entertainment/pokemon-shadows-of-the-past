using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPokedexSectionBaseStats : MonoBehaviour
{
    public UIStat statPrefab;
    public TextMeshProUGUI helpText;

    public Transform statList;
    public UIPokedexSectionBaseStats Load(PokedexPokemonData pokemonData)
    {
        PokemonBaseData pokemon = pokemonData.pokemon;
        foreach(Transform stat in statList)
        {
            Destroy(stat.gameObject);
        }
        if (pokemonData.caughtAmount > 0)
        {
            Instantiate(statPrefab, statList).Load("Attack", "" + pokemon.baseStats.attack);
            Instantiate(statPrefab, statList).Load("Defense", "" + pokemon.baseStats.defense);
            Instantiate(statPrefab, statList).Load("Sp. Attack", "" + pokemon.baseStats.spAttack);
            Instantiate(statPrefab, statList).Load("Sp. Defense", "" + pokemon.baseStats.spDefense);
            Instantiate(statPrefab, statList).Load("Speed", "" + pokemon.baseStats.speed);
            helpText.text = "";
        }
        else
        {
            helpText.text = "Catch this pokemon to learn more about it.";
        }
        return this;
    }
}

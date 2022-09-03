using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPokedexEvolution : MonoBehaviour
{
    public TextMeshProUGUI condition;
    public Image icon;

    public UIPokedexEvolution Load(PokemonBaseEvolution evolution, bool hasSeen, bool hasSeenEvo)
    {
        PokemonBaseData evo = PokemonMaster.GetInstance().GetPokemonData(evolution.pokemonId);
        icon.sprite = evo.icon;
        if (hasSeenEvo)
        {
            icon.color = Color.white;
        }
        else
        {
            icon.color = Color.black;
        }
        if (!hasSeen)
        {
            condition.text = "No Data";
        }
        else if (evolution.evolutionData.evolutionType == PokemonEvolutionType.level)
        {
            condition.text = "Lv " + evolution.evolutionData.value;
        }
        else if (evolution.evolutionData.evolutionType == PokemonEvolutionType.level)
        {
            condition.text = "Love " + evolution.evolutionData.value;
        }
        else if (evolution.evolutionData.evolutionType == PokemonEvolutionType.item)
        {
            ItemData item = ItemMaster.GetInstance().GetItem((ItemId)(int)evolution.evolutionData.value);
            condition.text = "Use " + item.GetName();
        }
        return this;
    }
}

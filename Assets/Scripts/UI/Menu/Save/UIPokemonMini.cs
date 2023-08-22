using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPokemonMini : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI level;

    public UIPokemonMini Load(PokemonCaughtData pokemon)
    {
        icon.sprite = pokemon.GetIcon();
        if (level)
        {
            level.text = "Lv. " + pokemon.GetLevel();
        }
        return this;
    }

    public UIPokemonMini Load(PersistedPokemon pokemon)
    {
        PokemonBaseData pbd = PokemonMaster.GetInstance().GetPokemonData(pokemon.pokemonId);
        icon.sprite = pbd.GetIcon();
        if (level)
        {
            level.text = "Lv. " + pokemon.level;
        }
        return this;
    }
}

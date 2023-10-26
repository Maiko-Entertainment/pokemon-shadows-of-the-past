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
        if (pokemon == null) return this;
        icon.sprite = pokemon.GetIcon();
        if (level)
        {
            level.text = "Lv. " + pokemon.GetLevel();
        }
        return this;
    }

    public UIPokemonMini Load(PersistedPokemon pokemon)
    {
        if (pokemon == null) return this;
        PokemonBaseData pbd = PokemonMaster.GetInstance().GetPokemonData(pokemon.GetId());
        if (pbd == null) return this;
        icon.sprite = pbd.GetIcon();
        if (level)
        {
            level.text = "Lv. " + pokemon.level;
        }
        return this;
    }
}

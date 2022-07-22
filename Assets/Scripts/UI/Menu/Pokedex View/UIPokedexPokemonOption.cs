using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPokedexPokemonOption : MonoBehaviour, ISelectHandler
{
    public Image icon;
    public TextMeshProUGUI pokemonSpecies;
    public Image caughtIcon;
    public TransitionFade selectedArrow;

    public delegate void Hover(PokedexPokemonData pkmn);
    public event Hover onHover;
    public delegate void Click(PokedexPokemonData pkmn);
    public event Click onClick;

    public PokedexPokemonData pokemon;

    public UIPokedexPokemonOption Load(PokedexPokemonData data)
    {
        pokemon = data;
        icon.sprite = data.pokemon.icon;
        if (data.seenAmount > 0)
        {
            icon.color = Color.white;
            pokemonSpecies.text = data.pokemon.species;
        }
        else
        {
            icon.color = Color.black;
            pokemonSpecies.text = "???";
        }
        if (data.caughtAmount > 0)
        {
            caughtIcon.gameObject.SetActive(true);
        }
        else
        {
            caughtIcon.gameObject.SetActive(false);
        }
        return this;
    }

    public void UpdateSelected(PokedexPokemonData pkmn)
    {
        if (pokemon == pkmn)
            selectedArrow?.FadeIn();
        else
            selectedArrow?.FadeOut();
    }
    public void HandleClick()
    {
        onClick?.Invoke(pokemon);
    }

    public void HandleHover()
    {
        onHover?.Invoke(pokemon);
    }

    public void OnSelect(BaseEventData eventData)
    {
        HandleHover();
    }
}

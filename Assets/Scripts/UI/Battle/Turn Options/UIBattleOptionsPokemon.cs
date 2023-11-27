using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleOptionsPokemon : MonoBehaviour
{
    public Image pokemonIcon;
    public TextMeshProUGUI pokemonName;
    public Image healthbar;
    public TextMeshProUGUI healthValue;
    public TextMeshProUGUI level;
    public Image exphbar;
    public Image statusSimbol;
    public TransitionFade selectedArrow;
    public Color faintedColor;

    public delegate void Hover(PokemonBattleData pkmn);
    public event Hover onHover;
    public delegate void Click(PokemonBattleData pkmn);
    public event Click onClick;

    private PokemonBattleData pokemon;

    public UIBattleOptionsPokemon Load(PokemonBattleData pokemon)
    {
        this.pokemon = pokemon;
        return Load(pokemon);
    }
    public void UpdateStatus(StatusEffect currenStatus)
    {
        StatusEffectData status = currenStatus.effectData;
        UpdateStatus(status);
    }
    public void UpdateStatus(StatusEffectData status)
    {
        if (status)
        {
            statusSimbol.enabled = true;
            statusSimbol.sprite = status.icon;
            return;
        }
        statusSimbol.enabled = false;
    }

    public void UpdateSelected(PokemonBattleData pkmn)
    {
        if (pokemon == pkmn)
            selectedArrow?.FadeIn();
        else
            selectedArrow?.FadeOut();
    }

    public void OnClick()
    {
        onClick?.Invoke(pokemon);
    }

    public void ShowPreview()
    {
        onHover?.Invoke(pokemon);
    }
}

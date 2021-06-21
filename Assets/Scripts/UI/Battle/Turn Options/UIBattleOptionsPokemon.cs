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

    private PokemonBattleData pokemon;

    public void Load(PokemonBattleData pokemon)
    {
        this.pokemon = pokemon;
        pokemonIcon.sprite = pokemon.GetPokemonCaughtData().GetPokemonBaseData().icon;
        pokemonName.text = pokemon.GetName();
        float maxHealth = pokemon.GetPokemonHealth();
        float currentHealth = pokemon.GetPokemonCurrentHealth();
        healthbar.fillAmount = currentHealth / maxHealth;
        healthValue.text = currentHealth + "/"+ maxHealth;
    }

    public void OnClick()
    {
        if (!pokemon.IsFainted()){
            BattleMaster.GetInstance()?.GetCurrentBattle()?.SetTeamActivePokemon(pokemon, BattleTeamId.Team1);
            BattleAnimatorMaster.GetInstance()?.ClosePokemonSelection();
        }
    }
}

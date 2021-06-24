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
    public TransitionFade selectedArrow;
    public Color faintedColor;

    private PokemonBattleData pokemon;

    public void Load(PokemonBattleData pokemon)
    {
        this.pokemon = pokemon;
        pokemonIcon.sprite = pokemon.GetPokemonCaughtData().GetPokemonBaseData().icon;
        pokemonName.text = pokemon.GetName();
        pokemonName.color = pokemon.IsFainted() ? faintedColor : Color.white;
        float maxHealth = pokemon.GetPokemonHealth();
        float currentHealth = pokemon.GetPokemonCurrentHealth();
        healthbar.fillAmount = currentHealth / maxHealth;
        healthValue.text = currentHealth + "/"+ maxHealth;
    }

    public void UpdateSelected(PokemonBattleData pkmn)
    {
        if (this.pokemon == pkmn)
            selectedArrow?.FadeIn();
        else
            selectedArrow?.FadeOut();
    }

    public void OnClick()
    {
        if (!pokemon.IsFainted()){
            BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
            PokemonBattleData activePokemon = bm.GetTeamActivePokemon(BattleTeamId.Team1);
            if (activePokemon.IsFainted())
            {
                bm.AddSwitchInPokemonEvent(pokemon);
            }
            else
            {
                bm.HandleTurnInput(
                    new BattleTurnDesitionPokemonSwitch(
                        pokemon,
                        BattleTeamId.Team1
                    ));
            }
            BattleAnimatorMaster.GetInstance()?.HideOptions();
        }
    }

    public void ShowPreview()
    {
        BattleAnimatorMaster.GetInstance()?.ShowPokemonSelectionData(pokemon);
    }
}

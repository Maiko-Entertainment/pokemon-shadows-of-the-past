using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattlePokemonPickerManager : MonoBehaviour
{
    public UIBattleOptionsPokemon pokemonOptionPrefab;
    public TransitionFade screenContainer;
    public Transform pokemonList;

    public void ShowPokemonPicker()
    {
        screenContainer.FadeIn();
        foreach(Transform pkmn in pokemonList)
        {
            Destroy(pkmn.gameObject);
        }
        List<PokemonBattleData> pokemon = BattleMaster.GetInstance()?.GetCurrentBattle()?.team1.pokemon;
        foreach(PokemonBattleData pkmn in pokemon)
        {
            UIBattleOptionsPokemon pokemonOption = Instantiate(pokemonOptionPrefab.gameObject, pokemonList)
                .GetComponent<UIBattleOptionsPokemon>();
            pokemonOption.Load(pkmn);
        }
    }

    public void ClosePokemonPicker()
    {
        screenContainer.FadeOut();
    }
}

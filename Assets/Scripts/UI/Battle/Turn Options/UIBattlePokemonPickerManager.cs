using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UIBattlePokemonPickerManager : MonoBehaviour
{
    public UIBattleOptionsPokemon pokemonOptionPrefab;
    public UIBattleMove movePrefab;
    public UIBattleType typePrefab;
    public TransitionFade screenContainer;
    public Transform pokemonList;
    public Transform pokemonMoveList;
    public Transform pokemonTypesList;
    public Button closeButton;

    private PokemonBattleData currentlySelected;

    public void ShowPokemonPicker(bool allowClose=false)
    {
        BattleAnimatorMaster.GetInstance()?.HideOptions();

        closeButton.interactable = allowClose;
        screenContainer.FadeIn();
        int index = 0;
        foreach (Transform pkmn in pokemonList)
        {
            UIBattleOptionsPokemon pokemonOption = pkmn.GetComponent<UIBattleOptionsPokemon>();
            if (pokemonOption)
            {
                pokemonOption.onClick -= OnPokemonSelect;
                pokemonOption.onHover -= ShowPokemonPreview;
            }
            Destroy(pkmn.gameObject);
        }
        ShowPokemonPreview(BattleMaster.GetInstance()?.GetCurrentBattle()?.team1.GetFirstAvailabelPokemon());
        List<Selectable> options = new List<Selectable>();
        List<PokemonBattleData> pokemon = BattleMaster.GetInstance()?.GetCurrentBattle()?.team1.pokemon;
        foreach (PokemonBattleData pkmn in pokemon)
        {
            UIBattleOptionsPokemon pokemonOption = Instantiate(pokemonOptionPrefab.gameObject, pokemonList)
                .GetComponent<UIBattleOptionsPokemon>();
            pokemonOption.Load(pkmn);
            pokemonOption.onClick += OnPokemonSelect;
            pokemonOption.onHover += ShowPokemonPreview;
            pokemonOption.UpdateSelected(currentlySelected);
            if (!pkmn.IsFainted())
            {
                options.Add(pokemonOption.GetComponent<Selectable>());
            }
            if (currentlySelected == pkmn)
            {
                EventSystem eventSystem = EventSystem.current;
                eventSystem.SetSelectedGameObject(pokemonOption.gameObject, new BaseEventData(eventSystem));
            }
            index++;
        }
        UtilsMaster.LineSelectables(options);
    }

    public void UpdatePokemon()
    {
        foreach (Transform pkmn in pokemonList)
        {
            pkmn.GetComponent<UIBattleOptionsPokemon>().UpdateSelected(currentlySelected);
        }
    }

    public void ShowPokemonPreview(PokemonBattleData pokemon)
    {
        currentlySelected = pokemon;
        LoadPokemonMoves(pokemon);
        foreach (Transform types in pokemonTypesList)
        {
            Destroy(types.gameObject);
        }
        foreach (PokemonTypeId types in pokemon.GetPokemonCaughtData().GetTypes())
        {
            Instantiate(typePrefab.gameObject, pokemonTypesList).GetComponent<UIBattleType>().Load(types);
        }
        UpdatePokemon();
    }

    private void LoadPokemonMoves(PokemonBattleData pokemon)
    {
        List<MoveEquipped> moves = pokemon.GetMoves();
        foreach(Transform pokemonMove in pokemonMoveList)
        {
            Destroy(pokemonMove.gameObject);
        }
        foreach (MoveEquipped move in moves)
        {
            UIBattleMove moveInstance = Instantiate(movePrefab.gameObject, pokemonMoveList).GetComponent<UIBattleMove>().Load(move, pokemon);
            moveInstance.seeOnly = true;
        }
    }

    public void OnPokemonSelect(PokemonBattleData pokemon)
    {
        if (!pokemon.IsFainted())
        {
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

    public void ClosePokemonPicker()
    {
        BattleAnimatorMaster.GetInstance()?.HideOptions();
    }

    public void Hide()
    {
        screenContainer.FadeOut();
    }

    public void HandleCancel(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            Hide();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UIBattleOptionsManager : MonoBehaviour
{

    public TransitionFilledImage container;
    public UIBattleMovePicker movesSelector;
    public UIItemsViewer itemSelector;
    public UIPokemonView pokemonSelector;
    public Transform optionsList;
    public Transform subMenuContainer;
    public bool isInSubmenu = false;

    private UIItemsViewer itemSelectorInstance;
    private UIPokemonView pokemonSelectorInstance;

    private void Start()
    {
        List<Selectable> options = new List<Selectable>();
        foreach(Transform option in optionsList)
        {
            options.Add(option.GetComponent<Selectable>());
        }
        UtilsMaster.LineSelectables(options);
    }

    public void ShowMoveSelector()
    {
        BattleAnimatorMaster.GetInstance()?.HideOptions();
        movesSelector.Show();
        isInSubmenu = true;
    }
    public void HideMoveSelector(bool preSelect = false)
    {
        movesSelector.Hide();
        isInSubmenu = false; 
        if (preSelect)
            UtilsMaster.SetSelected(optionsList.GetChild(0).gameObject);
    }

    public void ShowItemPokemonSelector(ItemDataOnPokemon item)
    {
        isInSubmenu = true;
    }
    public void ShowPokemonSelector(bool allowClose = false)
    {
        BattleAnimatorMaster.GetInstance()?.HideOptions();
        pokemonSelectorInstance = Instantiate(pokemonSelector, subMenuContainer);
        pokemonSelectorInstance.SetPrefaint(!allowClose);
        isInSubmenu = true;
    }

    public void HidePokemonSelector(bool preSelect = false)
    {
        pokemonSelectorInstance?.GetComponent<UIMenuPile>().Close();
        pokemonSelectorInstance = null;
        if (preSelect)
            UtilsMaster.SetSelected(optionsList.GetChild(1).gameObject);
        isInSubmenu = false;
    }
    public void ShowItemSelector()
    {
        BattleAnimatorMaster.GetInstance()?.HideOptions();
        itemSelectorInstance = Instantiate(itemSelector, subMenuContainer);
        isInSubmenu = true;
    }

    public void HideItemSelector(bool preSelect = false)
    {
        itemSelectorInstance?.GetComponent<UIMenuPile>().Close();
        itemSelectorInstance = null;
        if (preSelect)
            UtilsMaster.SetSelected(optionsList.GetChild(2).gameObject);
        isInSubmenu = false;
    }

    public void Hide()
    {
        container.FadeOut();
    }

    public void Show()
    {
        container.FadeIn();
        UtilsMaster.SetSelected(optionsList.GetChild(0).gameObject);
    }

    public void TryToEscape()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        switch (bm.GetBattleData().battleType)
        {
            case BattleType.Trainer:
                BattleAnimatorMaster.GetInstance().ExecuteNoRunningFromTrainerFlowchart();
                break;
            default:
                bm.HandleTurnInput(new BattleTurnDesitionRun(BattleTeamId.Team1));
                break;

        }
    }

    public void HandleCancel(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            if (!isInSubmenu)
            {
                EventSystem eventSystem = EventSystem.current;
                eventSystem.SetSelectedGameObject(optionsList.GetChild(optionsList.childCount - 1).gameObject, new BaseEventData(eventSystem));
            }
        }
    }
}

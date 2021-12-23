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
    public UIBattleItemPickerManager itemSelector;
    public Transform optionsList;
    public bool isInSubmenu = false;

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
    public void HideMoveSelector()
    {
        movesSelector.Hide();
        isInSubmenu = false;
    }

    public void ShowItemPokemonSelector(ItemDataOnPokemon item)
    {
        itemSelector.ShowPokemonList(item);
        isInSubmenu = true;
    }
    public void ShowItemSelector()
    {
        BattleAnimatorMaster.GetInstance()?.HideOptions();
        itemSelector.ShowItemSelector();
        isInSubmenu = true;
    }

    public void HideItemSelector()
    {
        itemSelector.HideItemSelector();
        isInSubmenu = false;
    }

    public void Hide()
    {
        container.FadeOut();
    }

    public void Show()
    {
        container.FadeIn();
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(optionsList.GetChild(0).gameObject, new BaseEventData(eventSystem));
    }

    public void HandleCancel(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            if (isInSubmenu)
            {
                HideMoveSelector();
                EventSystem eventSystem = EventSystem.current;
                eventSystem.SetSelectedGameObject(optionsList.GetChild(0).gameObject, new BaseEventData(eventSystem));
            }
            else
            {
                EventSystem eventSystem = EventSystem.current;
                eventSystem.SetSelectedGameObject(optionsList.GetChild(optionsList.childCount - 1).gameObject, new BaseEventData(eventSystem));
            }
        }
    }
}

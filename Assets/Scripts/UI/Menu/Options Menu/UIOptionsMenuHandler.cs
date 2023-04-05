using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UIOptionsMenuHandler : MonoBehaviour
{
    public Transform optionsList;

    private void Start()
    {
        List<Selectable> selectables = new List<Selectable>();
        if (optionsList)
        {
            foreach(Transform option in optionsList)
            {
                if (option.GetComponent<Selectable>())
                {
                    selectables.Add(option.GetComponent<Selectable>());
                }
            }
            UtilsMaster.LineSelectables(selectables);
        }
    }
    public void Close()
    {
        UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
    }
    public void Close(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            if (BattleMaster.GetInstance().GetCurrentBattle().IsBattleActive())
            {
                BattleAnimatorMaster.GetInstance().HideOptions();
            }
            else
            {
                UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
            }
        }
    }
    public void CloseIfCurrent(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            if (UIPauseMenuMaster.GetInstance().GetCurrentMenu() == GetComponent<UIMenuPile>())
            {
                UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
            }
        }
    }

    public void TryToOpenMenu(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            UIPauseMenuMaster.GetInstance().OpenMenu();
        }
    }
    public void OpenPokedexView()
    {
        UIPauseMenuMaster.GetInstance().OpenPokedexViewer();
    }
    public void OpenPokemonView()
    {
        UIPauseMenuMaster.GetInstance().OpenPokemonViewer();
    }
    public void OpenItemView()
    {
        UIPauseMenuMaster.GetInstance().OpenItemsViewer();
    }
    public void OpenSaveFileView()
    {
        UIPauseMenuMaster.GetInstance().OpenSaveMenu();
    }
}

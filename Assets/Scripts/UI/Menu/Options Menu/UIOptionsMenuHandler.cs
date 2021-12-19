using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class UIOptionsMenuHandler : MonoBehaviour
{
    public void Close()
    {
        UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
    }
    public void Close(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
        }
    }

    public void TryToOpenMenu(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            UIPauseMenuMaster.GetInstance().OpenMenu();
        }
    }

    public void OpenPokemonView()
    {
        UIPauseMenuMaster.GetInstance().OpenPokemonViewer();
    }
    public void OpenItemView()
    {
        UIPauseMenuMaster.GetInstance().OpenItemsViewer();
    }
}

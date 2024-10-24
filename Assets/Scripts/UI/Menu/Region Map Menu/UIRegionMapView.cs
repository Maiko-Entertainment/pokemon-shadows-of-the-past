using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class UIRegionMapView : MonoBehaviour
{
    public UIMap map;

    private void Start()
    {
        map.OpenMap(true);
    }

    public void HandleClose(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            HandleClose();
        }
    }

    public void HandleClose()
    {
        if (UIPauseMenuMaster.GetInstance().GetCurrentMenu() == GetComponent<UIMenuPile>())
            UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
    }
}

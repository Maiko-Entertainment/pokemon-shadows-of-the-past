using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOptionsMenuHandler : MonoBehaviour
{
    public void Close()
    {
        UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
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

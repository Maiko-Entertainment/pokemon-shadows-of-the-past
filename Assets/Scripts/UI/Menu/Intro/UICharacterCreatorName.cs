using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterCreatorName : MonoBehaviour
{
    public UIMenuPile nextMenu;
    public void OnSubmit(string characterName)
    {
        if (characterName != "")
        {
            SaveMaster.Instance.SaveElement("playerName", characterName);
            UIPauseMenuMaster.GetInstance()?.OpenMenu(nextMenu, true);
        }
    }
}

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
            SaveMaster.Instance.SetSaveElement(characterName, CommonSaveElements.playerName);
            UIPauseMenuMaster.GetInstance()?.OpenMenu(nextMenu, true);
        }
    }
}

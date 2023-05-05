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
            SaveElement playerName = SaveMaster.Instance.GetSaveElement(SaveElementId.playerName);
            playerName.SetValue(characterName);
            UIPauseMenuMaster.GetInstance()?.OpenMenu(nextMenu, true);
        }
    }
}

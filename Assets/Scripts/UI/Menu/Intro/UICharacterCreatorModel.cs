using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UICharacterCreatorModel : MonoBehaviour
{
    public UIMenuPile nextMenu;
    public UICharacterPicker pickerPrefab;
    public Transform characterList;


    private void Start()
    {
        Load();
        UIMenuPile menu = GetComponent<UIMenuPile>();
        if (menu)
        {
            menu.onActivateMenu += () => Load();
        }
    }

    private void Load()
    {
        foreach (Transform t in characterList)
        {
            Destroy(t.gameObject);
        }
        int playerAmounts = WorldMapMaster.GetInstance().playerPrefabs.Count;
        SaveElement characterPicked = SaveMaster.Instance.GetSaveElement(SaveElementId.characterModelId);
        int currentPick = (int)(float) characterPicked.GetValue();
        for (int i = 0; i < playerAmounts; i++)
        {
            UICharacterPicker cPicker = Instantiate(pickerPrefab, characterList).Load(i);
            cPicker.onClick += OnPick;
            if (i == currentPick)
                StartCoroutine(UtilsMaster.SetSelectedNextFrame(cPicker.gameObject));
        }
    }

    public void OnPick(float modeId)
    {
        SaveElement characterPicked = SaveMaster.Instance.GetSaveElement(SaveElementId.characterModelId);
        characterPicked.SetValue(modeId);
        UIPauseMenuMaster.GetInstance()?.OpenMenu(nextMenu, true);
    }

    public void HandleGoBack(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
        }
    }
}

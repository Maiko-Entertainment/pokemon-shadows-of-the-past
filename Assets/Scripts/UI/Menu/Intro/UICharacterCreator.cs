using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterCreator : MonoBehaviour
{
    public UICharacterPicker pickerPrefab;

    public AudioClip openMenuSound;
    public AudioClip submitSound;

    public TMP_InputField nameInput;
    public Transform characterList;
    public Button submitcharacterData;

    void Start()
    {
        Load();
    }

    // Update is called once per frame
    public void Load()
    {
        foreach (Transform t in characterList)
        {
            Destroy(t.gameObject);
        }
        int playerAmounts = WorldMapMaster.GetInstance().playerPrefabs.Count;
        for (int i = 0; i < playerAmounts; i++)
        {
            Instantiate(pickerPrefab, characterList).Load(i);
        }
    }
    public void SubmitPlayerData()
    {
        SaveElement playerName = SaveMaster.Instance.GetSaveElementData(CommonSaveElements.playerName);
        playerName.SetValue(nameInput.text);
    }
}

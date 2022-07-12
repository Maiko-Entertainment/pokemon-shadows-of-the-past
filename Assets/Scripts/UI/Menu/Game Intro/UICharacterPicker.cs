using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterPicker : MonoBehaviour
{
    public Color neutralColor = Color.white;
    public Color pickedColor;
    public Image chracterPreview;
    public Image background;
    public AudioClip selectedSound;

    private float chraracterIndex = 0;
    public UICharacterPicker Load(int chraracterIndex)
    {
        this.chraracterIndex = chraracterIndex;
        chracterPreview.sprite = WorldMapMaster.GetInstance().GetPlayerPrefab(chraracterIndex).preview;
        return this;
    }

    public void Pick()
    {
        SaveElement characterPicked = SaveMaster.Instance.GetSaveElement(SaveElementId.characterModelId);
        AudioMaster.GetInstance()?.PlaySfx(selectedSound);
        characterPicked.SetValue(chraracterIndex);
    }

    private void Update()
    {
        SaveElement characterPicked = SaveMaster.Instance.GetSaveElement(SaveElementId.characterModelId);
        if (chraracterIndex == (float)characterPicked.GetValue())
        {
            background.color = pickedColor;
        }
        else
        {
            background.color = neutralColor;
        }
    }
}

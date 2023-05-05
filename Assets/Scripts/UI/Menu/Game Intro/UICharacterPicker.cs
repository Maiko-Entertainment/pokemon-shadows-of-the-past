using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterPicker : MonoBehaviour, ISelectHandler
{
    public Image chracterPreview;
    public AudioClip selectedSound;

    public delegate void OnClick(float characterIndex);
    public event OnClick onClick;
    public delegate void OnHover(float characterIndex);
    public event OnHover onHover;

    private float chraracterIndex = 0;
    public UICharacterPicker Load(int chraracterIndex)
    {
        this.chraracterIndex = chraracterIndex;
        chracterPreview.sprite = WorldMapMaster.GetInstance().GetPlayerPrefab(chraracterIndex).preview;
        return this;
    }

    public void HandleClick()
    {
        AudioMaster.GetInstance()?.PlaySfx(selectedSound);
        onClick?.Invoke(chraracterIndex);
    }

    public void OnSelect(BaseEventData eventData)
    {
        onHover?.Invoke(chraracterIndex);
    }
}

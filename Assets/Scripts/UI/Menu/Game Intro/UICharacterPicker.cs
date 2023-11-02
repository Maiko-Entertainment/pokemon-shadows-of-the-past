using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterPicker : MonoBehaviour, ISelectHandler
{
    public Image chracterPreview;
    public AudioClip selectedSound;

    public delegate void OnClick(int characterIndex);
    public event OnClick onClick;
    public delegate void OnHover(int characterIndex);
    public event OnHover onHover;

    private int chraracterIndex = 0;
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

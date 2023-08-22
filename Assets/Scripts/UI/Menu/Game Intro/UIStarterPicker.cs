using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStarterPicker : MonoBehaviour, ISelectHandler
{
    public Image chracterPreview;
    public Image background;
    public AudioClip selectedSound;

    public delegate void Click(PokemonBaseData starter);
    public event Click OnClick;
    public delegate void OnSelectStarter(PokemonBaseData starter);
    public event OnSelectStarter onSelect;

    private PokemonBaseData starter;

    public UIStarterPicker Load(PokemonBaseData starter)
    {
        this.starter = starter;
        chracterPreview.sprite = starter.GetIcon();
        return this;
    }

    public void Pick()
    {
        OnClick?.Invoke(starter);
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioMaster.GetInstance()?.PlaySfx(starter.GetCry());
        onSelect?.Invoke(starter);
    }
}

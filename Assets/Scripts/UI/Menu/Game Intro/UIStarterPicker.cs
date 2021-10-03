using UnityEngine;
using UnityEngine.UI;

public class UIStarterPicker : MonoBehaviour
{
    public Color neutralColor = Color.white;
    public Color pickedColor;
    public Image chracterPreview;
    public Image background;
    public AudioClip selectedSound;

    public delegate void Click(PokemonBaseData starter);
    public event Click OnClick;

    private PokemonBaseData starter;

    public UIStarterPicker Load(PokemonBaseData starter)
    {
        this.starter = starter;
        chracterPreview.sprite = starter.icon;
        return this;
    }

    public void Pick()
    {
        SaveElement characterPicked = SaveMaster.Instance.GetSaveElement(SaveElementId.starterPickedId);
        AudioMaster.GetInstance()?.PlaySfx(selectedSound);
        AudioMaster.GetInstance()?.PlaySfx(starter.GetCry());
        characterPicked.SetValue((float)starter.pokemonId);
        OnClick?.Invoke(starter);
    }

    private void Update()
    {
        SaveElement characterPicked = SaveMaster.Instance.GetSaveElement(SaveElementId.starterPickedId);
        if ((float)starter.pokemonId == (float)characterPicked.GetValue())
        {
            background.color = pickedColor;
        }
        else
        {
            background.color = neutralColor;
        }
    }
}

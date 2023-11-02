using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISaveFileOption : MonoBehaviour, ISelectHandler
{
    public Image playerIcon;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI saveIdendex;
    public UIPokemonMini pokemonPartyPrefab;

    public Transform persistedPokemonList;

    public delegate void Hover(SaveFile saveFile, int index);
    public event Hover onHover;
    public delegate void Click(SaveFile saveFile, int index);
    public event Hover onClick;

    protected SaveFile saveFile;
    protected int index;

    public UISaveFileOption Load(SaveFile saveFile, int index)
    {
        this.saveFile = saveFile;
        this.index = index;
        string playerNameSaved = SaveMaster.Instance.GetElementAsString("playerName");
        List<PokemonElement> persistedPokemon = saveFile.partyElements;

        Sprite previewSprite = WorldMapMaster.GetInstance().GetPlayerPrefab(SaveMaster.Instance.GetElementAsInt("characterModelId")).preview;
        playerIcon.sprite = previewSprite;
        playerName.text = playerNameSaved;
        if (saveIdendex) saveIdendex.text = "" + (index + 1)+ ".";

        foreach (PokemonElement pp in persistedPokemon)
        {
            Instantiate(pokemonPartyPrefab, persistedPokemonList).Load(pp);
        }

        return this;
    }

    public void HandleClick()
    {
        onClick?.Invoke(saveFile, index);
    }

    public void OnSelect(BaseEventData eventData)
    {
        onHover?.Invoke(saveFile, index);
    }
}

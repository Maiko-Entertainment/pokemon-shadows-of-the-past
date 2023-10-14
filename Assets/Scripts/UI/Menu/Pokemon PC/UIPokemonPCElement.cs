using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPokemonPCElement : MonoBehaviour, ISelectHandler
{
    public Image icon;
    public GameObject cursor;

    public AudioClip onClickSound;

    public delegate void Hover(PokemonCaughtData caught);
    public event Hover onHover;
    public delegate void Click(PokemonCaughtData caught);
    public event Click onClick;

    public PokemonCaughtData pokemon;

    protected bool isEmpty = false;

    public UIPokemonPCElement Load(PokemonCaughtData pokemon)
    {
        this.pokemon = pokemon;
        icon.sprite = pokemon.GetIcon();
        return this;
    }
    public UIPokemonPCElement LoadEmpty()
    {
        isEmpty = true;
        icon.color = new Color(0, 0, 0, 0);
        return this;
    }

    public void HandleClick()
    {
        if (onClickSound)
        {
            AudioMaster.GetInstance().PlaySfx(onClickSound);
        }
        onClick?.Invoke(pokemon);
    }

    public void HandleHover()
    {
        onHover?.Invoke(pokemon);
    }

    public void OnSelect(BaseEventData eventData)
    {
        HandleHover();
    }

    public void UpdateSwaping(PokemonCaughtData pokemon)
    {
        icon.gameObject.SetActive(this.pokemon != pokemon);
    }

    public void SetCursorActive(bool value)
    {
        cursor.gameObject.SetActive(value);
    }
    public bool IsEmpty()
    {
        return isEmpty;
    }
}

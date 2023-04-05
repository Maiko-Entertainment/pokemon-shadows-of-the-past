using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPokedexEnvironment : MonoBehaviour
{
    public Image icon;
    public UIPokedexEnvironment Load(PokemonBaseDataAreasEnvironment area)
    {
        icon.sprite = area.environmentIcons;
        return this;
    }
}

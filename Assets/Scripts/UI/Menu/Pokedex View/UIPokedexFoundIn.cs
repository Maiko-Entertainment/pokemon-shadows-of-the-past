using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPokedexFoundIn : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Transform environmentDayList;
    public Transform environmentNightList;

    public UIPokedexEnvironment environmentDataPrefab;

    public UIPokedexFoundIn Load(PokedexPokemonData pokemonData, PokemonBaseDataAreas a)
    {
        foreach (Transform area in environmentDayList)
        {
            Destroy(area.gameObject);
        }
        foreach (Transform area in environmentNightList)
        {
            Destroy(area.gameObject);
        }
        title.text = a.encounteredIn.title;
        foreach (PokemonBaseDataAreasEnvironment e in a.environments)
        {
            if (e.dayType == TimeOfDayType.Day || e.dayType == TimeOfDayType.Any)
            {
                Instantiate(environmentDataPrefab, environmentDayList).Load(e);
            }
            if (e.dayType == TimeOfDayType.Night || e.dayType == TimeOfDayType.Any)
            {
                Instantiate(environmentDataPrefab, environmentNightList).Load(e);
            }
        }
        return this;
    }
}

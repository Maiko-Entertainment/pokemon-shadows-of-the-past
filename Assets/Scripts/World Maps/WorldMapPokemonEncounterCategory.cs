using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Encounters/Pokemon Wild Encounter Category")]
public class WorldMapPokemonEncounterCategory : ScriptableObject
{
    public string categoryId = "";
    public string categoryName = "";
    public Sprite icon;

    public string GetId() { return categoryId; }

}

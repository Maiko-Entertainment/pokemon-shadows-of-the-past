using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public List<PersistedSaveElement> persistedElements = new List<PersistedSaveElement>();
    public List<PersistedItem> persistedItems = new List<PersistedItem>();
    public List<PersistedPokedexPokemonData> persistedPokedexPokemonData = new List<PersistedPokedexPokemonData>();
    public List<PersistedPokemon> persistedParty = new List<PersistedPokemon>();
    public List<PersistedPokemon> persistedBox = new List<PersistedPokemon>();
    public List<PersistedTactic> persistedTacticsEquipped = new List<PersistedTactic>();
    public List<PersistedTactic> persistedTactics = new List<PersistedTactic>();
    public int playerMapId = 2;
    public SerializableVector2 playerPos = new SerializableVector2(Vector2.zero);
    public int lastSafeZoneMapId = 3;
    public int lastSafeZoneIndex = 0;
    public float musicVolume = 0.5f;
    public float soundVolume = 0.5f;
}

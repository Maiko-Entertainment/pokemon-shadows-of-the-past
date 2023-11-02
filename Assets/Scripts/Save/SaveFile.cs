using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public int fileVersion = 1;
    public List<ObjectElement> elements = new List<ObjectElement>();
    public List<ItemElement> itemsElements = new List<ItemElement>();
    public List<PokedexPokemonDataElement> pokedexPokemonDataElements = new List<PokedexPokemonDataElement>();
    public List<PokemonElement> partyElements = new List<PokemonElement>();
    public List<PokemonElement> boxElements = new List<PokemonElement>();
    public List<TacticElement> tacticsEquippedElements = new List<TacticElement>();
    public List<TacticElement> tacticsElements = new List<TacticElement>();
    public int playerMapId = 2;
    public SerializableVector2 playerPos = new SerializableVector2(Vector2.zero);
    public int lastSafeZoneMapId = 3;
    public int lastSafeZoneIndex = 0;
    public float musicVolume = 0.5f;
    public float soundVolume = 0.5f;
}

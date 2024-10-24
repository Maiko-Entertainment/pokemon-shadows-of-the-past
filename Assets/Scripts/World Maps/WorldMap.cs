using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class WorldMap : MonoBehaviour
{
    public int mapId = 0;
    public string title;
    public int defaultSafePlacePosIndex;
    public List<Transform> spawnList = new List<Transform>();
    public AudioOptions mapMusic;
    public TransitionBase titleCard;
    public VolumeProfile volumeProfile; 

    public Tilemap groundTilemap;
    public Tilemap waterTilemap;
    public Tilemap collisionTilemap;

    public List<WorldMapTimeOfDayEffect> timeOfDayEffects = new List<WorldMapTimeOfDayEffect>();
    public bool noSave = false;
    public bool isMainMenu = false;
    public List<WorldMapPokemonEncounter> pokemonEncounters = new List<WorldMapPokemonEncounter>();

    public void HandleEntrance()
    {
        AudioMaster.GetInstance().PlayMusic(mapMusic);
        if (titleCard != null)
            WorldMapMaster.GetInstance().CreateTitleCard(titleCard);
        CreateDayEffects();
        TransitionMaster.GetInstance()?.SetCameraProfile(volumeProfile);
    }

    public WorldMapPokemonEncounter GetWorldMapPokemonEncounter(WorldMapPokemonEncounterCategory category)
    {
        foreach(WorldMapPokemonEncounter encounter in pokemonEncounters)
        {
            if (encounter.category == category) return encounter;
        }
        return null;
    }

    public void CreateDayEffects()
    {
        TransitionMaster.GetInstance().ClearDayEffects();
        foreach (WorldMapTimeOfDayEffect tode in timeOfDayEffects)
        {
            tode.Initiate();
        }
    }

    public void HandleReturn()
    {
        AudioMaster.GetInstance().PlayMusic(mapMusic);
        CreateDayEffects();
    }

    public Transform GetSpawn(int index)
    {
        return spawnList[index];
    }

    public AudioOptions GetMapMusic()
    {
        return mapMusic;
    }
}

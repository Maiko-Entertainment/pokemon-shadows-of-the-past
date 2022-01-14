using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldMap : MonoBehaviour
{
    public int mapId = 0;
    public string title;
    public List<Transform> spawnList = new List<Transform>();
    public AudioClip mapMusic;
    public TransitionBase titleCard;

    public Tilemap groundTilemap;
    public Tilemap waterTilemap;
    public Tilemap collisionTilemap;

    public List<WorldMapTimeOfDayEffect> timeOfDayEffects = new List<WorldMapTimeOfDayEffect>();

    public void HandleEntrance()
    {
        AudioMaster.GetInstance().PlayMusic(mapMusic);
        if (titleCard != null)
            WorldMapMaster.GetInstance().CreateTitleCard(titleCard);
        CreateDayEffects();
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

    public AudioClip GetMapMusic()
    {
        return mapMusic;
    }
}

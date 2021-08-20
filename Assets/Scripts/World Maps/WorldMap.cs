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

    public void HandleEntrance()
    {
        AudioMaster.GetInstance().PlayMusic(mapMusic);
        if (titleCard != null)
            WorldMapMaster.GetInstance().CreateTitleCard(titleCard);
    }

    public void HandleReturn()
    {
        AudioMaster.GetInstance().PlayMusic(mapMusic);
    }

    public Transform GetSpawn(int index)
    {
        return spawnList[index];
    }
}

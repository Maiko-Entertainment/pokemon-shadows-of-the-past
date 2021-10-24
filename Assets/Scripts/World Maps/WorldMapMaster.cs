using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapMaster : MonoBehaviour
{
    public static WorldMapMaster Instance;

    public List<PlayerController> playerPrefabs;
    public List<WorldMap> maps;
    public List<SceneMap> scenes;
    public Transform sceneContainer;
    public Transform mapNameContainer;
    public TimeOfDayType timeOfDay;

    public bool forceMapLoad = false;
    public int customMap = 2;
    public int customSpawn = 0;

    protected WorldMap currentMap;
    protected SceneMap currentScene;
    protected PlayerController player;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public static WorldMapMaster GetInstance() { return Instance; }

    private void Start()
    {
        if (forceMapLoad)
        {
            GoToMap(customMap, customSpawn);
        }
    }

    public void GoToMap(int mapId, int spawnIndex)
    {
        ClearPrevius();
        WorldMap map = Getmap(mapId);
        WorldMap mapInstance = Instantiate(map.gameObject).GetComponent<WorldMap>();
        Transform spawn = mapInstance.GetSpawn(spawnIndex);
        mapInstance.HandleEntrance();
        PlayerController player = GetPlayer();
        player.transform.position = spawn.position;
        player.Load(mapInstance);
        UIPauseMenuMaster.GetInstance().ShowWorldUI();
        currentMap = mapInstance;
    }

    public WorldMap Getmap(int mapId)
    {
        foreach(WorldMap wm in maps)
        {
            if (wm.mapId == mapId)
                return wm;
        }
        return null;
    }

    public WorldMap GetCurrentMap()
    {
        return currentMap;
    }

    public PlayerController GetPlayer()
    {
        if (player == null)
        {
            SaveElement se = SaveMaster.Instance.GetSaveElement(SaveElementId.characterModelId);
            int index = (int)(float)se.GetValue();
            player = Instantiate(playerPrefabs[index].gameObject).GetComponent<PlayerController>();
        }
        return player;
    }

    public PlayerController GetPlayer(int index)
    {
        return playerPrefabs[index].gameObject.GetComponent<PlayerController>();
    }

    public void HandleMapReturn()
    {
        currentMap?.HandleReturn();
    }

    public void CreateTitleCard(TransitionBase titleCard)
    {
        StartCoroutine(HideTitleCard(titleCard));
    }

    IEnumerator HideTitleCard(TransitionBase titleCard)
    {
        foreach (Transform title in mapNameContainer)
            Destroy(title.gameObject);
        yield return new WaitForSeconds(1f);
        TransitionBase titleCardInstance = Instantiate(titleCard, mapNameContainer).GetComponent<TransitionBase>();
        titleCardInstance.FadeIn();
    }

    public void GoToScene(int sceneId)
    {
        ClearPrevius();
        SceneMap map = GetScene(sceneId);
        SceneMap mapInstance = Instantiate(map.gameObject).GetComponent<SceneMap>();
        currentScene = mapInstance;
    }

    public SceneMap GetScene(int sceneId)
    {
        foreach (SceneMap wm in scenes)
        {
            if (wm.sceneId == sceneId)
                return wm;
        }
        return null;
    }
    public void ClearPrevius()
    {
        if (currentMap != null)
            Destroy(currentMap?.gameObject);
        if (currentScene != null)
            Destroy(currentScene?.gameObject);
    }

    public void SetTimeOfDay(TimeOfDayType type)
    {
        timeOfDay = type;
    }

    public TimeOfDayType GetTimeOfDay()
    {
        return timeOfDay;
    }

    public void PlayCurrentPlaceMusic()
    {
        if (currentMap)
        {
            AudioClip mapMusic = currentMap.GetMapMusic();
            AudioMaster.GetInstance().PlayMusic(mapMusic);
        }
    }
}

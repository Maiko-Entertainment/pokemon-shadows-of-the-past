using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapMaster : MonoBehaviour
{
    public static WorldMapMaster Instance;

    public PlayerController playerPrefab;
    public List<WorldMap> maps;
    public List<SceneMap> scenes;
    public Transform sceneContainer;
    public Transform mapNameContainer;
    public TimeOfDayType timeOfDay;

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
            GetPlayer();
        }
    }

    public static WorldMapMaster GetInstance() { return Instance; }

    private void Start()
    {
        GoToMap(0, 1);
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

    public PlayerController GetPlayer()
    {
        if (player == null)
            player = Instantiate(playerPrefab.gameObject).GetComponent<PlayerController>();
        return player;
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
        float stayTime = 3;
        yield return new WaitForSeconds(stayTime);
        float fadeOutAfter = 1f / titleCardInstance.speed;
        titleCardInstance.FadeOut();
        Destroy(titleCardInstance.gameObject, fadeOutAfter);
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
}

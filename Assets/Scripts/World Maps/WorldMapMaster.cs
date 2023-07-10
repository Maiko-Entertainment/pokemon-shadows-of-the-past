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

    public void Load(SaveFile save)
    {
        SpawnInMapAtPos(save.playerMapId, save.playerPos.GetVector2());
    }

    public void HandleSave()
    {
        SaveMaster.Instance.activeSaveFile.playerMapId = currentMap.mapId;
        SaveMaster.Instance.activeSaveFile.playerPos = new SerializableVector2(GetPlayer().gameObject.transform.position);
    }

    public void GoToMapToLatestSafePoint()
    {
        SaveFile sf = SaveMaster.Instance.GetActiveSave();
        if (sf != null)
        {
            int mapId = sf.lastSafeZoneMapId;
            int safeIndex = sf.lastSafeZoneIndex;
            GoToMap(mapId, safeIndex);
        }
    }

    public void GoToMap(int mapId, int spawnIndex)
    {
        StartCoroutine(GoToMapCourutine(mapId, spawnIndex));
    }

    public IEnumerator GoToMapCourutine(int mapId, int spawnIndex)
    {
        ClearPrevius();
        yield return new WaitForEndOfFrame();
        Transform spawn = Getmap(mapId).GetSpawn(spawnIndex);
        Vector3 position = spawn.position;
        SpawnInMapAtPos(mapId, position);
    }

    public void SpawnInMapAtPos(int mapId, Vector2 position)
    {
        ClearPrevius();
        StartCoroutine(SpawnInMapPosCoroutine(mapId, position));
    }

    public IEnumerator SpawnInMapPosCoroutine(int mapId, Vector2 position)
    {
        yield return new WaitForEndOfFrame();
        WorldMap map = Getmap(mapId);
        StartCoroutine(SetPlayer(position));
        WorldMap mapInstance = Instantiate(map.gameObject).GetComponent<WorldMap>();
        currentMap = mapInstance;
        mapInstance.HandleEntrance();
        if (mapInstance.isMainMenu)
        {
            Destroy(GetPlayer().gameObject);
            UIPauseMenuMaster.GetInstance().HideWorldUI();
            UIPauseMenuMaster.GetInstance().OpenMainMenu();
        }
        else
        {
            UIPauseMenuMaster.GetInstance().ShowWorldUI();
        }

    }

    IEnumerator SetPlayer(Vector2 spawn)
    {
        PlayerController player = GetPlayer();
        player?.SetPosition(spawn);
        yield return new WaitForEndOfFrame();
        if (player)
        {
            player?.SetPosition(spawn);
            player?.Load(currentMap);
        }
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

    public PlayerController GetPlayerPrefab(int index)
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
        SaveMaster.Instance.SetSaveElementInner(type, SaveElementId.timeOfDay);
    }

    public TimeOfDayType GetTimeOfDay()
    {
        SaveElement pse = SaveMaster.Instance.GetSaveElement(SaveElementId.timeOfDay);
        TimeOfDayType day = (TimeOfDayType)(float)pse.GetValue();
        return day;
    }

    public void PlayCurrentPlaceMusic()
    {
        if (currentMap)
        {
            AudioOptions mapMusic = currentMap.GetMapMusic();
            AudioMaster.GetInstance().PlayMusic(mapMusic);
        }
    }

    public void AddInstancedFollowerToPlayer(WorldInteractableBrainFollower follower, bool repeatable)
    {
        GetPlayer()?.AddFollowerInstanced(follower, repeatable);
    }

    public WorldInteractableBrainFollower RemoveFollowerFromPlayer(string followerId)
    {
        WorldInteractableBrainFollower follower = GetPlayer()?.RemoveFollower(followerId);
        return follower;
    }
}

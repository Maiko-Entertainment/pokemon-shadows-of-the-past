using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMap : MonoBehaviour
{
    public List<UIMapLocation> locations = new List<UIMapLocation>();
    public GameObject cursor;
    public GameObject playerIcon;
    public UIMapLocationInfo locationInfo;

    public bool openOnStart = false;

    public delegate void UIMapLocationDelegate(UIMapLocation location);
    public event UIMapLocationDelegate onLocationView;
    public event UIMapLocationDelegate onLocationSelect;

    private void Start()
    {
        if (openOnStart)
        {
            OpenMap(true);
        }
    }

    public void InitiateLocations()
    {
        foreach (UIMapLocation location in locations)
        {
            location.onHover += () => HandleCursorLocationChange(location.mapId);
        }
    }

    public void OpenMap(bool showCursor)
    {
        if (cursor != null)
        {
            cursor?.SetActive(showCursor);
        }
        InitiateLocations();
        UpdatePlayerPosition();
        SelectCurrentLocation();
    }

    public void UpdatePlayerPosition()
    {
        WorldMap currentMap = WorldMapMaster.GetInstance().GetCurrentMap();
        if (currentMap == null)
        {
            Console.Error.WriteLine("Current Map is null");
        }
        foreach (UIMapLocation location in locations)
        {
            if (currentMap.mapId == location.mapId) {
                playerIcon.transform.position = location.transform.position;
            }
        }
    }

    public void SelectCurrentLocation()
    {
        if (!IsCursorActive())
        {

            return;
        }
        WorldMap currentMap = WorldMapMaster.GetInstance().GetCurrentMap();
        if (currentMap == null)
        {
            Console.Error.WriteLine("Current Map is null");
        }
        UIMapLocation location = FindLocation(currentMap.mapId);
        if (location != null)
        {
            HandleCursorLocationChange(location.mapId);
            UtilsMaster.SetSelected(location.gameObject);
        }
    }

    public void HandleCursorLocationChange(int locationId)
    {
        if (!IsCursorActive())
        {
            return;
        }
        WorldMap currentMap = WorldMapMaster.GetInstance().Getmap(locationId);
        if (currentMap == null)
        {
            Console.Error.WriteLine("Current Map is null");
        }
        UIMapLocation location = FindLocation(locationId);
        cursor.transform.position = location.transform.position;
        if (locationInfo)
        {
            locationInfo?.GetComponent<UIMenuAnim>().OpenDialog();
            locationInfo.LoadMapInfo(currentMap);
        }
    }

    public bool IsCursorActive() { return cursor != null && cursor.activeInHierarchy; }

    public UIMapLocation FindLocation(int locationId)
    {
        foreach (UIMapLocation location in locations)
        {
            if (locationId == location.mapId)
            {
                return location;
            }
        }
        return null;
    }
}

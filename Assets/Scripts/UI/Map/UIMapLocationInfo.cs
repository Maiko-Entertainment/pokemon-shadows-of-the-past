using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMapLocationInfo : MonoBehaviour
{
    public TextMeshProUGUI mapTitle;


    public void LoadMapInfo(WorldMap map)
    {
        mapTitle.text = map.title;
    }
}

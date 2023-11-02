using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPokemonSettings : MonoBehaviour
{
    public Transform settingsList;

    protected UIPokemonView viewer;

    public void Load(UIPokemonView viewer)
    {
        this.viewer = viewer;
        LinkOptions();
    }

    public void LinkOptions()
    {
        UtilsMaster.LineSelectables(new List<Selectable>(settingsList.GetComponentsInChildren<Selectable>()));
        UtilsMaster.SetSelected(settingsList.GetChild(0).gameObject);
    }

    public void Rename()
    {
        viewer.SetNameChanger();
    }
}

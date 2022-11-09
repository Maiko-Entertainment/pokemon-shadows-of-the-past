using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatChange : MonoBehaviour
{
    public TextMeshProUGUI statName;
    public TextMeshProUGUI statValue;
    public Image container;
    public Color upColor;
    public Color downColor;

    public UIStatChange Load(string statName, int statStage)
    {
        string statStageText = "+" + statStage;
        container.color = upColor;
        if (statStage < 0)
        {
            statStageText = ""+statStage;
            container.color = downColor;
        }
        this.statName.text = statName;
        this.statValue.text = statStageText;
        return this;
    }
}

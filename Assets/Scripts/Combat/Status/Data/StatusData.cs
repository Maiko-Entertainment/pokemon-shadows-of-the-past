using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusData : ScriptableObject
{
    public string id;
    public string statusName;
    public Sprite icon;


    public Flowchart flowchart;
    public string onStartStatusBlockName = "";
    public string onTriggerFlowchartBlock = "";
    public string onEndFlowchartBlock = "";

    public int minTurnDuration = 2;
    public int extraTurnRange = 0;

    [Range(-1f, 1f)]
    public float percentageDamagePerRound = 0f;
    public TriggerCondition percentageDamageConditions = new TriggerCondition();


    public virtual string GetId()
    {
        return id;
    }

    public virtual string GetStatusName()
    {
        return statusName;
    }

    public Sprite GetIcon()
    {
        return icon;
    }
}

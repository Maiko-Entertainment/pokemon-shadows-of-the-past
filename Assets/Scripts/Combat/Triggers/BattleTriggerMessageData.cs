using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BattleTriggerMessageData
{
    public Flowchart flowchart;
    public string blockName;
    public Dictionary<string, string> variables;

    public BattleTriggerMessageData(
        Flowchart flowchart, 
        string blockName, 
        Dictionary<string,string> variables = null)
    {
        this.flowchart = flowchart;
        this.blockName = blockName;
        if (variables != null)
            this.variables = variables;
        else
            this.variables = new Dictionary<string, string>();
    }
}

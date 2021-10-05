using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractableList : WorldInteractable
{
    public Flowchart flowchartPrefab;
    public List<string> blockNames = new List<string>() { "Start" };

    public override void OnInteract()
    {
        base.OnInteract();
        foreach(string block in blockNames)
            InteractionsMaster.GetInstance()?.AddEvent(new InteractionEventFlowchart(flowchartPrefab, block));
    }
}

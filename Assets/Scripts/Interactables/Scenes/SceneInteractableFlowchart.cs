using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInteractableFlowchart : SceneInteractable
{
    public Flowchart flowChartInstance;
    public string blockName = "Start";
    public override void Interact()
    {
        base.Interact();
        InteractionsMaster.GetInstance()?.AddEvent(
            new InteractionEventFlowchart(
                flowChartInstance,
                blockName
        ));
    }
}

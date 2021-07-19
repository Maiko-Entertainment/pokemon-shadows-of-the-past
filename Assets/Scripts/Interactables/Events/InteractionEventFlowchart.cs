using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEventFlowchart : InteractionEvent
{
    public Flowchart flowchart;
    public string blockName;

    public InteractionEventFlowchart(Flowchart flowchart, string blockName)
    {
        this.flowchart = flowchart;
        this.blockName = blockName;
    }

    public override void Execute()
    {
        base.Execute();
        flowchart?.ExecuteBlock(blockName);
    }
}

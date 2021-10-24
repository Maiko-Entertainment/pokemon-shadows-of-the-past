using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEventCutscene : InteractionEvent
{
    public Cutscene cutscene;
    public InteractionEventCutscene(Cutscene cutscene)
    {
        this.cutscene = cutscene;
    }

    public override void Execute()
    {
        cutscene.Initiate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEventFinishEvolution : InteractionEvent
{
    public InteractionEventFinishEvolution()
    {
        priority = 10f;
    }

    public override void Execute()
    {
        UIEvolutionMaster.GetInstance().FinishEvolution();
    }
}

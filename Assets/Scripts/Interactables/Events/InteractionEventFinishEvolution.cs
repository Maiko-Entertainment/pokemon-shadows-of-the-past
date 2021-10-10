using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEventFinishEvolution : InteractionEvent
{
    public InteractionEventFinishEvolution()
    {

    }

    public override void Execute()
    {
        UIEvolutionMaster.GetInstance().FinishEvolution();
    }
}

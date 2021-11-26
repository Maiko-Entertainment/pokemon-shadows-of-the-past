using System.Collections.Generic;
using UnityEngine;

public class InteractionEventDestroyInstance : InteractionEvent
{
    List<GameObject> instances;
    public InteractionEventDestroyInstance(List<GameObject> instances)
    {
        this.instances = instances;
    }

    public override void Execute()
    {
        foreach(GameObject instance in instances)
            GameObject.Destroy(instance);
        InteractionsMaster.GetInstance().ExecuteNext(0);
    }
}

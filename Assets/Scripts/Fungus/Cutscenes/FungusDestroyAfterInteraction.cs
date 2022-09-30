using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo(
    "Cutscene",
    "Destroy after Interaction",
    "Destroys a gameobject after the interaction is over"
)]
public class FungusDestroyAfterInteraction : Command
{
    public List<GameObject> gameObjects = new List<GameObject>();
    public override void OnEnter()
    {
        InteractionsMaster.GetInstance().AddEvent(new InteractionEventDestroyInstance(gameObjects));
        Continue();
    }

    public override Color GetButtonColor()
    {
        return new Color32(140, 52, 235, 255);
    }

    public override string GetSummary()
    {
        return base.GetSummary();
    }
}

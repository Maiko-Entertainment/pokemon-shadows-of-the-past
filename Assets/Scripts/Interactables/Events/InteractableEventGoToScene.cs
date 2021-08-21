using UnityEngine;

public class InteractableEventGoToScene : InteractionEvent
{
    public WorldInteractableGoToScene interactableWorld;

    public InteractableEventGoToScene(WorldInteractableGoToScene interactableWorld)
    {
        this.interactableWorld = interactableWorld;
    }

    public override void Execute()
    {
        base.Execute();
        interactableWorld.Execute();
    }
}

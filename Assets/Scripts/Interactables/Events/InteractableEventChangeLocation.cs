using UnityEngine;

public class InteractableEventChangeLocation : InteractionEvent
{
    WorldInteractableChangeLocation worldInteraction;

    public InteractableEventChangeLocation(WorldInteractableChangeLocation worldInteraction)
    {
        this.worldInteraction = worldInteraction;
    }

    public override void Execute()
    {
        worldInteraction?.Execute();
    }
}

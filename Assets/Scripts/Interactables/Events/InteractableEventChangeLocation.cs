using UnityEngine;

public class InteractableEventChangeLocation : InteractionEvent
{
    WorldInteractableChangeLocation worldInteraction;
    SceneInteractableGoToWorldMap sceneInteraction;

    public InteractableEventChangeLocation(WorldInteractableChangeLocation worldInteraction)
    {
        this.worldInteraction = worldInteraction;
    }
    public InteractableEventChangeLocation(SceneInteractableGoToWorldMap sceneInteraction)
    {
        this.sceneInteraction = sceneInteraction;
    }

    public override void Execute()
    {
        worldInteraction?.Execute();
        sceneInteraction?.Execute();
    }
}

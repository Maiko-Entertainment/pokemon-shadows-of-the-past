using UnityEngine;

public class InteractableEventChangeLocation : InteractionEvent
{
    WorldInteractableChangeLocation interaction;
    public InteractableEventChangeLocation(WorldInteractableChangeLocation interaction)
    {
        this.interaction = interaction;
    }

    public override void Execute()
    {
        interaction.Execute();
    }
}

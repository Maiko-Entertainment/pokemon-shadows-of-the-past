using Fungus;

public class WorldInteractableTouch : WorldInteractable
{
    public Flowchart flowchartPrefab;
    public string blockName = "Start";

    public override void OnInteract()
    {
        base.OnInteract();
        InteractionsMaster.GetInstance()?.AddEvent(new InteractionEventFlowchart(flowchartPrefab, blockName));
    }
}

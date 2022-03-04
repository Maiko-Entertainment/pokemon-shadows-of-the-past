using Fungus;
using UnityEngine;

public class WorldInteractableTouch : WorldInteractable
{
    public Flowchart flowchartPrefab;
    public string blockName = "Start";

    public override void OnInteract()
    {
        base.OnInteract();
        InteractionsMaster.GetInstance()?.AddEvent(new InteractionEventFlowchart(flowchartPrefab, blockName));
        if (moveBrain)
        {
            Vector3 position = WorldMapMaster.GetInstance().GetPlayer().transform.position;
            Vector3 myPosition = transform.position;
            Vector3 dir = (position - myPosition).normalized;
            moveBrain?.animator.SetFloat("Horizontal", dir.x);
            moveBrain?.animator.SetFloat("Vertical", dir.y);
        }

    }
}

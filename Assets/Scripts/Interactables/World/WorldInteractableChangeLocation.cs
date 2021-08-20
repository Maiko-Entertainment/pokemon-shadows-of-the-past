using System.Collections;
using UnityEngine;

public class WorldInteractableChangeLocation : WorldInteractable
{
    public int mapId = 0;
    public int spawnIndex = 0;
    public ViewTransition changeMapTransition;
    public override void OnInteract()
    {
        base.OnInteract();
        InteractionsMaster.GetInstance().AddEvent(new InteractableEventChangeLocation(this));
    }

    public virtual void Execute()
    {
        TransitionMaster.GetInstance().RunTransition(changeMapTransition);
        StartCoroutine(RunEvent(changeMapTransition.changeTime));
    }

    IEnumerator RunEvent(float delay)
    {
        yield return new WaitForSeconds(delay);
        WorldMapMaster.GetInstance().GoToMap(mapId, spawnIndex);
        InteractionsMaster.GetInstance()?.ExecuteNext(0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInteractableGoToWorldMap : SceneInteractable
{
    public int mapId;
    public int spawnIndex;

    public ViewTransition transition;

    public override void Interact()
    {
        base.Interact();
        InteractionsMaster.GetInstance().AddEvent(new InteractableEventChangeLocation(this));
    }

    public virtual void Execute()
    {
        TransitionMaster.GetInstance().RunToWorldTransition(transition);
        StartCoroutine(RunEvent(transition.changeTime));
    }

    IEnumerator RunEvent(float delay)
    {
        yield return new WaitForSeconds(delay);
        WorldMapMaster.GetInstance().GoToMap(mapId, spawnIndex);
        InteractionsMaster.GetInstance()?.ExecuteNext(0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractableGoToScene : WorldInteractable
{
    public int sceneId = 0;
    public ViewTransition changeMapTransition;
    public override void OnInteract()
    {
        base.OnInteract();
        InteractionsMaster.GetInstance().AddEvent(new InteractableEventGoToScene(this));
    }

    public virtual void Execute()
    {
        TransitionMaster.GetInstance().RunTransition(changeMapTransition);
        StartCoroutine(RunEvent(changeMapTransition.changeTime));
    }
    IEnumerator RunEvent(float delay)
    {
        yield return new WaitForSeconds(delay);
        WorldMapMaster.GetInstance().GoToScene(sceneId);
        InteractionsMaster.GetInstance()?.ExecuteNext(0);
    }
}

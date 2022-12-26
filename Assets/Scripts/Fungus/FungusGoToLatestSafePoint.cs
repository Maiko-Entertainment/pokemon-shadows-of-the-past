using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "World",
    "Go To latest safe zone",
    ""
)]

public class FungusGoToLatestSafePoint : Command
{
    public ViewTransition changeMapTransition;
    public bool triggerNextInteractionOnEnd = true;
    public override void Execute()
    {
        if (changeMapTransition)
        {
            TransitionMaster.GetInstance().ClearTransitions();
            TransitionMaster.GetInstance().RunTransition(changeMapTransition);
            StartCoroutine(RunEvent(changeMapTransition.changeTime));
        }
        else
        {
            StartCoroutine(RunEvent(0));
        }
    }

    IEnumerator RunEvent(float delay)
    {
        yield return new WaitForSeconds(delay);
        WorldMapMaster.GetInstance()?.GoToMapToLatestSafePoint();
        if (triggerNextInteractionOnEnd)
        {
            InteractionsMaster.GetInstance().ExecuteNext();
        }
        else
        {
            Continue();
        }
    }

    public override Color GetButtonColor()
    {
        return new Color32(42, 176, 49, 255);
    }

    public override string GetSummary()
    {
        return "Go to latest safe zone";
    }
}

using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Transitions",
    "Play Transition",
    "Plays a transition using Transition Master. Empty to clear transitions."
)]
public class FungusPlayTransition : Command
{
    public ViewTransition transition;
    public bool destroyprevious = true;
    public bool waitForEnd = false;
    public override void OnEnter()
    {
        if (destroyprevious)
        {
            TransitionMaster.GetInstance().ClearTransitions();
        }
        if (transition)
        {
            TransitionMaster.GetInstance().RunTransition(transition);
            if (waitForEnd)
            {
                StartCoroutine(ContinueAfter(transition.changeTime));
            }
            else
            {
                Continue();
            }
        }
        else
        {
            TransitionMaster.GetInstance().ClearTransitions();
            Continue();
        }
    }
    IEnumerator ContinueAfter(float time)
    {
        yield return new WaitForSeconds(time);
        Continue();
    }
    public override string GetSummary()
    {
        string clipName = transition != null ? transition.name : "Clear transitions.";
        return clipName;
    }
}

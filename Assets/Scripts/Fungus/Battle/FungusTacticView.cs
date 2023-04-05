using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Combat",
    "Show Tactic Transition",
    "Shows the tactics transition."
)]
public class FungusTacticView : Command
{
    public ViewTransition tacticTransition;
    public override void OnEnter()
    {
        TransitionMaster.GetInstance().ClearTransitions();
        TransitionMaster.GetInstance().RunTransition(tacticTransition);
        StartCoroutine(RunEvent(tacticTransition.totalDuration));
    }

    IEnumerator RunEvent(float delay)
    {
        yield return new WaitForSeconds(delay);
        Continue();
    }
}

using UnityEngine;
using Fungus;
using System.Collections.Generic;
using System.Collections;

[CommandInfo(
    "Cutscene",
    "Move Agent",
    ""
)]

public class FungusCutsceneMoveAgent : Command
{
    public string agentId;
    public bool overwriteSpeed = false;
    public float speed = 3;
    public List<MoveBrainDirection> directions = new List<MoveBrainDirection>();
    public bool waitUntilEnd = true;
    public override void OnEnter()
    {
        Cutscene cs = CutsceneMaster.GetInstance().GetCurrentCutscene();
        WorldInteractable wi = cs.GetAgent(agentId);
        float finalTime = 0;
        if (wi)
        {
            WorldInteractableMoveBrain mb = wi.moveBrain;
            if (overwriteSpeed)
                mb.speed = speed;
            foreach (MoveBrainDirection mbd in directions)
                finalTime = mb.AddDirection(mbd);
            if (waitUntilEnd)
            {
                StartCoroutine(ContinueAfter(finalTime));
            }
            else
            {
                Continue();
            }
        }
        else
        {
            Continue();
        }
    }

    IEnumerator ContinueAfter(float time)
    {
        yield return new WaitForSeconds(time);
        Continue();
    }

    public override Color GetButtonColor()
    {
        return new Color32(140, 52, 235, 255);
    }

    public override string GetSummary()
    {
        return base.GetSummary();
    }
}

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
    public List<MoveBrainDirectionData> directions = new List<MoveBrainDirectionData>();
    public bool waitUntilEnd = true;
    public override void OnEnter()
    {
        float finalTime = 0;
        if (agentId == "player")
        {
            foreach(MoveBrainDirectionData direction in directions)
            {
                finalTime = WorldMapMaster.GetInstance().GetPlayer().AddDirection(direction);
            }
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
            Cutscene cs = CutsceneMaster.GetInstance().GetCurrentCutscene();
            WorldInteractable wi = cs.GetAgent(agentId);
            if (wi)
            {
                WorldInteractableMoveBrain mb = wi.moveBrain;
                if (overwriteSpeed)
                    mb.speed = speed;
                foreach (MoveBrainDirectionData mbd in directions)
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

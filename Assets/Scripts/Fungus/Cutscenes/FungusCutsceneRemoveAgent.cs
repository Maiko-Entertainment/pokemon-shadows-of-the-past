using UnityEngine;
using Fungus;

[CommandInfo(
    "Cutscene",
    "Remove Agent",
    "Destroys agents GameObject"
)]

public class FungusCutsceneRemoveAgent : Command
{
    public string agentId;
    public override void OnEnter()
    {
        Cutscene cs = CutsceneMaster.GetInstance().GetCurrentCutscene();
        WorldInteractable wi = cs.GetAgent(agentId);
        if (wi)
        {
            cs.cutsceneAgents.Remove(wi);
            Destroy(wi.gameObject);
        }
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

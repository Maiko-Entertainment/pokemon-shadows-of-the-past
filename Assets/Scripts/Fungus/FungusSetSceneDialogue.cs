using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
[CommandInfo(
    "Narrative",
    "Set Say Dialog to Scene",
    "Sets Say Dialog to Scene"
)]
public class FungusSetSceneDialogue : Command
{
    public override void OnEnter()
    {
        Continue();
        TransitionMaster.GetInstance()?.SetDialogueToScene();
    }

    public override Color GetButtonColor()
    {
        return new Color32(61, 170, 191, 255);
    }
}

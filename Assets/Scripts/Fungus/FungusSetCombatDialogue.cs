using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
[CommandInfo(
    "Narrative",
    "Set Say Dialog to Combat",
    "Sets Say Dialog to Combat"
)]
public class FungusSetCombatDialogue : Command
{
    public override void OnEnter()
    {
        Continue();
        TransitionMaster.GetInstance()?.SetDialogueToBattle();
    }

    public override Color GetButtonColor()
    {
        return new Color32(61, 170, 191, 255);
    }
}

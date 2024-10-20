﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
[CommandInfo(
    "Interactions",
    "Trigger Next Interaction",
    "Triggers next interaction master event"
)]
public class FungusTriggerNextInteraction : Command
{
    public override void OnEnter()
    {
        InteractionsMaster.GetInstance().ExecuteNext(0f);
        Continue();
    }

    public override Color GetButtonColor()
    {
        return new Color32(61, 170, 191, 255);
    }
}

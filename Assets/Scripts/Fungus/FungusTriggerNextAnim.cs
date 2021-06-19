using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
[CommandInfo(
    "Battle",
    "Trigger Next Anim",
    "Triggers next battle animation event"
)]
public class FungusTriggerNextAnim : Command
{
    public override void OnEnter()
    {
        Continue();
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim();
    }

    public override Color GetButtonColor()
    {
        return new Color32(61, 170, 191, 255);
    }
}

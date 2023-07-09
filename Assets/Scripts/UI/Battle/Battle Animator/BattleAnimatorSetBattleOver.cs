using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorSetBattleOver : BattleAnimatorEvent
{
    public BattleAnimatorSetBattleOver()
    {
        eventType = BattleAnimatorEventType.System;
    }

    public override void Execute()
    {
        BattleMaster.GetInstance().GetCurrentBattle()?.SetBattleActive(false);
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim();
        base.Execute();
    }
}

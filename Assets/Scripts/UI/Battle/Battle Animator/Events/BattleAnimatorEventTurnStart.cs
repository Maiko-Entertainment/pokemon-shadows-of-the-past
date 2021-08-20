using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventTurnStart : BattleAnimatorEvent
{
    public override void Execute()
    {
        base.Execute();
        BattleAnimatorMaster.GetInstance().HandleCameraIdle();
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim(0);
    }
}

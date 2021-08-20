using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventRoundEnd : BattleEvent
{
    public BattleEventRoundEnd() : base()
    {
        eventId = BattleEventId.roundEnd;
    }

    public override void Execute()
    {
        base.Execute();
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventTurnStart());
    }
}

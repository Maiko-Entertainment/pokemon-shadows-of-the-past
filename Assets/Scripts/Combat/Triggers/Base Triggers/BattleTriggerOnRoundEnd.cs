using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnRoundEnd : BattleTrigger
{
    public BattleTriggerOnRoundEnd() : base()
    {
        eventId = BattleEventId.roundEnd;
    }

    public virtual bool Execute(BattleEventRoundEnd roundEnd)
    {
        return base.Execute(roundEnd);
    }
}

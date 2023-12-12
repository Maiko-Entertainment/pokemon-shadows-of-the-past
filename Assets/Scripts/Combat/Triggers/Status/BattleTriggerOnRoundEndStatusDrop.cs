using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnRoundEndStatusDrop : BattleTriggerOnRoundEnd
{
    public Status status;

    public BattleTriggerOnRoundEndStatusDrop(Status status): base()
    {
        this.status = status;
    }

    public override bool Execute(BattleEventRoundEnd battleEvent)
    {
        status.PassRound();
        return base.Execute(battleEvent);
    }
}

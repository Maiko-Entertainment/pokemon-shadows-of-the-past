using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnRoundEndStatusDrop : BattleTriggerOnRoundEnd
{
    public StatusEffect status;

    public BattleTriggerOnRoundEndStatusDrop(StatusEffect status): base()
    {
        this.status = status;
    }

    public override bool Execute(BattleEventRoundEnd battleEvent)
    {
        status.PassTurn();
        return base.Execute(battleEvent);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnDesitionStatusDrop : BattleTriggerOnDesition
{
    public Status status;
    public BattleTriggerOnDesitionStatusDrop(Status status, BattleTeamId teamId): base(teamId)
    {
        this.status = status;
    }

    public override bool Execute(BattleEventDestion battleEvent)
    {
        if (teamId == battleEvent.desition.team)
        {
            status.PassTurn();
        }
        return base.Execute(battleEvent);
    }
}

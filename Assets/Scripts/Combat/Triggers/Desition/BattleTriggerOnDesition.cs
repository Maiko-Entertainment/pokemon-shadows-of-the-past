using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnDesition : BattleTrigger
{
    public BattleTeamId teamId;

    public BattleTriggerOnDesition(BattleTeamId teamId)
    {
        this.teamId = teamId;
    }
    public virtual bool Execute(BattleEventDestion battleEvent)
    {
        return base.Execute(battleEvent);
    }
}

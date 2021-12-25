using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventTactic : BattleEvent
{
    public TacticData tactic;
    public BattleTeamId teamId;
    public BattleEventTactic(TacticData tactic, BattleTeamId teamId) : base()
    {
        eventId = BattleEventId.useTactic;
        this.tactic = tactic;
        this.teamId = teamId;
    }

    public override void Execute()
    {
        tactic.Execute(teamId);
        base.Execute();
    }
}

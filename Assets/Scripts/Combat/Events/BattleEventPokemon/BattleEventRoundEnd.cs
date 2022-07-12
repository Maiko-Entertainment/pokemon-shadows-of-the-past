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
        BattleTeamData team1Data = BattleMaster.GetInstance().GetCurrentBattle().GetTeamData(BattleTeamId.Team1);
        BattleMaster.GetInstance().GetCurrentBattle().SetSelectedTactic(null);
        team1Data.IncreaseTacticGauge(1);
        BattleMaster.GetInstance().GetCurrentBattle().eventManager.RemoveEndOfRoundTriggers();
        BattleMaster.GetInstance().GetCurrentBattle().turnsPassed++;
    }
}

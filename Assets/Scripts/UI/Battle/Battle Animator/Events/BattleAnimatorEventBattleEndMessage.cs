using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventBattleEndMessage : BattleAnimatorEvent
{
    public BattleEventBattleEnd battleEvent;

    public BattleAnimatorEventBattleEndMessage(BattleEventBattleEnd battleEvent)
    {
        this.battleEvent = battleEvent;
        eventType = BattleAnimatorEventType.BattleDescriptionText;
    }

    public override void Execute()
    {
        if (battleEvent.battleTeamId == BattleTeamId.Team1)
        {
            BattleAnimatorMaster.GetInstance().ExecuteEnemyTrainerDefeated(battleEvent.finalBattleState.team2.trainerTitle);
        }
        else if (battleEvent.battleTeamId == BattleTeamId.Team2)
        {
            BattleAnimatorMaster.GetInstance().ExecuteEnemyTrainerDefeated(battleEvent.finalBattleState.team1.trainerTitle);
        }
        base.Execute();
    }
}

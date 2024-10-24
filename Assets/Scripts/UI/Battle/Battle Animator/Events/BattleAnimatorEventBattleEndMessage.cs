﻿using System.Collections;
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
        if (battleEvent.winningTeamId == BattleTeamId.Team1)
        {
            if (battleEvent.finalBattleState.battleData.battleWonMusic)
            {
                AudioMaster.GetInstance().PlayMusic(battleEvent.finalBattleState.battleData.battleWonMusic);
            }
            BattleAnimatorMaster.GetInstance().ExecuteEnemyTrainerDefeated(battleEvent.finalBattleState.team2.trainerTitle);
        }
        else if (battleEvent.winningTeamId == BattleTeamId.Team2)
        {
            BattleAnimatorMaster.GetInstance().ExecuteEnemyTrainerDefeated(battleEvent.finalBattleState.team1.trainerTitle);
        }
        else
        {
            if (battleEvent.finalBattleState.GetBattleData().battleType == BattleType.Pokemon)
            {
                BattleAnimatorMaster.GetInstance().ExecuteBattleEscape(battleEvent.finalBattleState.team1.trainerTitle);
            }
        }
        base.Execute();
    }
}

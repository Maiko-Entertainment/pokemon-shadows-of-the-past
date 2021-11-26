﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventBattleEnd : BattleEvent
{
    public BattleTeamId winningTeamId;
    public BattleManager finalBattleState;
    public BattleEndEvent endEvent;
    public BattleEventBattleEnd(BattleManager finalBattleState, BattleTeamId winningTeamId, BattleEndEvent endEvent)
    {
        eventId = BattleEventId.battleEnd;
        this.winningTeamId = winningTeamId;
        this.finalBattleState = finalBattleState;
        this.endEvent = endEvent;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventBattleEndMessage(this));
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventEndBattle(this));
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventCheckEvolution());
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleEventPostBattleFungus(endEvent, this));
        base.Execute();
    }
}

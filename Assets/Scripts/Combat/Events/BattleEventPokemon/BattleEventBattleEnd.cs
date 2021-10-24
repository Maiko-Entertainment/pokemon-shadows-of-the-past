using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventBattleEnd : BattleEvent
{
    public BattleTeamId battleTeamId;
    public BattleManager finalBattleState;
    public BattleEventBattleEnd(BattleManager finalBattleState, BattleTeamId battleTeamId)
    {
        eventId = BattleEventId.battleEnd;
        this.battleTeamId = battleTeamId;
        this.finalBattleState = finalBattleState;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventBattleEndMessage(this));
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventEndBattle(this));
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventCheckEvolution());
        base.Execute();
    }
}

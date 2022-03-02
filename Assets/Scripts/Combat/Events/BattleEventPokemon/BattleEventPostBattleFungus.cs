using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPostBattleFungus: BattleAnimatorEvent
{
    BattleEndEvent endEvent;
    BattleEventBattleEnd e;
    public BattleEventPostBattleFungus(BattleEndEvent endEvent, BattleEventBattleEnd e)
    {
        this.endEvent = endEvent;
        this.e = e;
        eventType = BattleAnimatorEventType.PokemonInfoChange;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance().ClearEvents();
        endEvent?.Execute(e);
        base.Execute();
    }
}

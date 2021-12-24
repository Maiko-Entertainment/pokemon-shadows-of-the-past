using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventRun : BattleEvent
{
    public BattleEventRun()
    {
        eventId = BattleEventId.battleRun;
    }
    public override void Execute()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        bm.HandleTryToEscape();
    }
}

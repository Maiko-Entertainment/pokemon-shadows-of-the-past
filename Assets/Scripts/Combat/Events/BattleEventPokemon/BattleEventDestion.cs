using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Event happens before the choice is executed, used by statuses to decrease turns left (sleep, confused, etc)
public class BattleEventDestion : BattleEvent
{
    public BattleTurnDesition desition;
    public BattleEventDestion(BattleTurnDesition desition) : base()
    {
        this.desition = desition;
        eventId = BattleEventId.preDesition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventFieldStatusAdd : BattleEvent
{
    public StatusFieldData statusField;
    public BattleEventFieldStatusAdd(StatusFieldData statusField)
    {
        eventId = BattleEventId.pokemonAddField;
        this.statusField = statusField;
    }

    public override void Execute()
    {
        BattleMaster.GetInstance().GetCurrentBattle().AddStatusField(this);
        base.Execute();
    }
}

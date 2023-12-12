using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventFieldStatusRemove : BattleEvent
{
    public StatusFieldData statusField;
    public BattleEventFieldStatusRemove(StatusFieldData statusField)
    {
        this.statusField = statusField;
        eventId = BattleEventId.pokemonRemoveStatus;
    }

    public override void Execute()
    {
        BattleMaster.GetInstance().GetCurrentBattle().RemoveStatusField(statusField);
        // TODO: Update UI
    }
}

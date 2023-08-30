using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventNarrative : BattleEvent
{
    public BattleTriggerMessageData messageData;

    public BattleEventNarrative (BattleTriggerMessageData messageData) : base()
    {
        eventId = BattleEventId.dialogue;
        this.messageData = messageData;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance().AddEvent(
                new BattleAnimatorEventNarrative(messageData)
            );
    }
}

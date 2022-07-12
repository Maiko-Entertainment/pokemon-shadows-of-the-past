using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnRoundEndDialog : BattleTriggerOnRoundEnd
{
    public BattleTriggerMessageData messageData;
    public int roundNumber;
    public BattleTriggerOnRoundEndDialog(BattleTriggerMessageData messageData, int roundNumber) :
    base()
    {
        this.messageData = messageData;
        this.roundNumber = roundNumber;
        maxTriggers = 1;
    }

    public override bool Execute(BattleEventRoundEnd battleEvent)
    {
        int roundNumber = BattleMaster.GetInstance().GetCurrentBattle().turnsPassed;
        if (roundNumber == this.roundNumber)
        {
            if (maxTriggers > 0)
            {
                BattleAnimatorMaster.GetInstance().AddEvent(
                    new BattleAnimatorEventNarrative(
                            messageData
                    )
                );
            }
        }
        else
        {
            maxTriggers++;
        }
        return base.Execute(battleEvent);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleAnimatorEventNarrative : BattleAnimatorEvent
{
    public BattleTriggerMessageData messageData;

    public BattleAnimatorEventNarrative(BattleTriggerMessageData messageData) :
        base()
    {
        this.messageData = messageData;
        eventType = BattleAnimatorEventType.BattleDescriptionText;
    }

    public override void Execute()
    {
        messageData.flowchart.StopAllBlocks();
        foreach (string varName in messageData.variables.Keys.ToList())
        {
            messageData.flowchart.SetStringVariable(varName, messageData.variables[varName]);
        }
        messageData.flowchart.ExecuteBlock(messageData.blockName);
        base.Execute();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEvent
{
    public BattleAnimatorEventType eventType;
    public bool dontWait = false;

    public virtual void Execute()
    {
        if (dontWait)
            BattleAnimatorMaster.GetInstance().animatorManager.TriggerNextEvent();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BattleAnimatorEvent
{
    public BattleAnimatorEventType eventType;
    public bool dontWait = false;

    public BattleAnimatorEvent(bool dontWait = false)
    {
        this.dontWait = dontWait;
    }

    public virtual void Execute()
    {
        if (dontWait)
            BattleAnimatorMaster.GetInstance().GoToNextBattleAnim();
    }

    public override string ToString()
    {
        return eventType.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BattleAnimatorManager
{
    public List<BattleAnimatorEvent> events = new List<BattleAnimatorEvent>();

    public void AddEvent(BattleAnimatorEvent newEvent)
    {
        events.Add(newEvent);
    }

    public void TriggerNextEvent()
    {
        if (events.Count > 0)
        {
            events[0].Execute();
            events.RemoveAt(0);
        }
        else
        {
            // Show turn selection UI
            BattleAnimatorMaster.GetInstance().ShowTurnOptions();
        }
    }
}

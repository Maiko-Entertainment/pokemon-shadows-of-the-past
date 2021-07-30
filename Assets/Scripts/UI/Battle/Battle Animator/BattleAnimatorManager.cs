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
        UIBattleEventDebugger.GetInstance()?.UpdateAnims();
        if (events.Count > 0)
        {
            if (events[0] != null)
            {
                BattleAnimatorEvent animEvent = events[0];
                events.RemoveAt(0);
                animEvent?.Execute();
            }
        }
        else
        {
            // Show turn selection UI
            BattleAnimatorMaster.GetInstance().ShowTurnOptions();
        }
    }
}

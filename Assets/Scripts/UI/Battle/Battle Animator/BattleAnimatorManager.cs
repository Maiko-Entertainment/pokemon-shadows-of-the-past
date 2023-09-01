using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BattleAnimatorManager
{
    public List<BattleAnimatorEvent> events = new List<BattleAnimatorEvent>();

    public bool isExecutingEvent = false;

    public void AddEvent(BattleAnimatorEvent newEvent)
    {
        bool priortyInserted = false;
        foreach (BattleAnimatorEvent ae in events)
        {
            int index = events.IndexOf(ae);
            if (index != 0 && ae.priority < newEvent.priority)
            {
                events.Insert(index, newEvent);
                priortyInserted = true;
                break;
            }
        }
        if (!priortyInserted)
        {
            events.Add(newEvent);
        }
    }

    public void TriggerNextEvent()
    {
        UIBattleEventDebugger.GetInstance()?.UpdateAnims();
        if (events.Count > 0 && events[0] != null)
        {
            BattleAnimatorEvent animEvent = events[0];
            isExecutingEvent = true;
            events.RemoveAt(0);
            animEvent?.Execute();
        }
        else
        {
            // Show turn selection UI
            isExecutingEvent = false;
            BattleAnimatorMaster.GetInstance().ShowTurnOptions();
        }
    }
}

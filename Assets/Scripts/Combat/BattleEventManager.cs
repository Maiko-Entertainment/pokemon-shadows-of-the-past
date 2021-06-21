using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BattleEventManager
{
    public Dictionary<BattleEventId, List<BattleTrigger>> battleEvents = new Dictionary<BattleEventId, List<BattleTrigger>>();
    public List<BattleEvent> eventsPile = new List<BattleEvent>();

    public void AddBattleTrigger(BattleTrigger trigger)
    {
        Debug.Log("Trigger Added - " + trigger.eventId);
        if (!battleEvents.ContainsKey(trigger.eventId))
        {
            battleEvents.Add(trigger.eventId, new List<BattleTrigger>());
        }
        battleEvents[trigger.eventId].Add(trigger);
    }

    public void RemoveBattleTrigger(BattleTrigger trigger)
    {
        if (battleEvents.ContainsKey(trigger.eventId))
        {
            battleEvents[trigger.eventId].Remove(trigger);
        }
    }

    public void AddEvent(BattleEvent battleEvent)
    {
        Debug.Log("Stacked Event - "+ battleEvent.ToString());
        eventsPile.Insert(0, battleEvent);
    }

    public BattleEvent GetNextEvent()
    {
        return eventsPile[0];
    }

    public void RemoveEvent(BattleEvent battleEvent)
    {
        eventsPile.Remove(battleEvent);
    }

    public void ResolveAllEventTriggers()
    {
        while(eventsPile.Count > 0)
        {
            HandleNextEventTriggers();
        }
        UIBattleEventDebugger.GetInstance()?.UpdateTriggers();
    }

    // An event may be modified or skipped by a trigger
    // If an event hasnt been skipped after all trigers it is performed.
    public void HandleNextEventTriggers()
    {
        BattleEvent next = GetNextEvent();
        BattleEventId id = next.eventId;
        List<BattleTrigger> triggers = GetTriggersForEvent(id);
        bool keepGoing = true;
        foreach(BattleTrigger bt in triggers)
        {
            Debug.Log("Trigger Activated - " + bt.eventId);
            switch (id)
            {
                case BattleEventId.pokemonChangeStats:
                    BattleTriggerOnPokemonChangeStats btpcs = 
                        (BattleTriggerOnPokemonChangeStats)bt;
                    BattleEventPokemonChangeStat bepcs = (BattleEventPokemonChangeStat)next;
                    keepGoing = btpcs.Execute(bepcs);
                break;
                case BattleEventId.pokemonEnter:
                    BattleTriggerOnPokemonEnter btpe = (BattleTriggerOnPokemonEnter)bt;
                    keepGoing = btpe.Execute((BattleEventEnterPokemon)next);
                    break;
                case BattleEventId.pokemonFaint:
                    BattleTriggerOnPokemonFaint btpf = (BattleTriggerOnPokemonFaint)bt;
                    keepGoing = btpf.Execute((BattleEventPokemonFaint)next);
                    break;
                case BattleEventId.roundEnd:
                    BattleTriggerOnPokemonRoundEnd btpte = (BattleTriggerOnPokemonRoundEnd)bt;
                    keepGoing = btpte.Execute((BattleEventRoundEnd)next);
                    break;
            }
            if (!keepGoing)
            {
                RemoveEvent(next);
                break;
            }
        }
        // Event was not cancelled
        if (keepGoing)
        {
            next.Execute();
            RemoveEvent(next);
            // Checks if a pokemon left the battlefield and removes triggers related to them
            if (id == BattleEventId.pokemonFaint || id == BattleEventId.pokemonSwitch)
            {
                List<BattleTrigger> newCleanUpTriggers = new List<BattleTrigger>();
                triggers = GetTriggersForEvent(BattleEventId.pokemonLeaveCleanUp);
                foreach (BattleTrigger bt in triggers)
                {
                    BattleTriggerCleanUp btCleanUp = ((BattleTriggerCleanUp)bt);
                    btCleanUp.Execute((BattleEventPokemon)next);
                    if (!btCleanUp.cleanUpDone)
                        newCleanUpTriggers.Add(btCleanUp);
                }
                if (battleEvents.ContainsKey(BattleEventId.pokemonLeaveCleanUp))
                {
                    battleEvents[BattleEventId.pokemonLeaveCleanUp] = newCleanUpTriggers;
                }
            }
        }
    }

    public List<BattleTrigger> GetTriggersForEvent(BattleEventId eventId)
    {
        if (battleEvents.ContainsKey(eventId))
        {
            return battleEvents[eventId];
        }
        return new List<BattleTrigger>();
    }
}

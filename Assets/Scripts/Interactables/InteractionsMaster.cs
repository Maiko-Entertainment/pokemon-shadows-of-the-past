using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionsMaster : MonoBehaviour
{
    public static InteractionsMaster Instance;

    public List<InteractionEvent> events = new List<InteractionEvent>();

    private bool isInteracting = false;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public static InteractionsMaster GetInstance()
    {
        return Instance;
    }

    public void AddEvent(InteractionEvent intEvent)
    {
        print("Adding event: " + intEvent);
        bool priortyInserted = false;
        foreach(InteractionEvent ie in events)
        {
            int index = events.IndexOf(ie);
            if (index != 0 && ie.priority < intEvent.priority)
            {
                events.Insert(index, intEvent);
                priortyInserted = true;
                break;
            }
        }
        if (!priortyInserted)
        {
            events.Add(intEvent);
        }
        isInteracting = true;
        if (events.Count == 1)
        {
            events[0].Execute();
            UIPauseMenuMaster.GetInstance().HideMenuOpener();
        }
    }

    public void ExecuteNext(float time = 0f)
    {
         StartCoroutine(ExecuteAfterDelay(time));
    }

    private IEnumerator ExecuteAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        print("Events before cheking " + events.Count);
        if (events.Count > 0)
        {
            print("Executing event: " + events[0]);
            events.RemoveAt(0);
            if (events.Count > 0)
                events[0].Execute();
            else
            {
                isInteracting = false;
                if (!BattleMaster.GetInstance().IsBattleActive())
                    UIPauseMenuMaster.GetInstance().ShowMenuOpener();
            }
        }
        else
        {
            isInteracting = false;
            if (!BattleMaster.GetInstance().IsBattleActive())
                UIPauseMenuMaster.GetInstance().ShowMenuOpener();
        }
    }

    public bool IsInteractionPlaying()
    {
        bool isBattleHappening = BattleMaster.GetInstance().IsBattleActive();
        bool isMenuOpen = UIPauseMenuMaster.GetInstance().IsMenuOpen();
        bool isTransitionHappening = TransitionMaster.GetInstance().IsTransitioning();
        return isInteracting || isBattleHappening || isMenuOpen || isTransitionHappening;
    }
    public bool IsInteracting()
    {
        bool isBattleHappening = BattleMaster.GetInstance().IsBattleActive();
        bool isTransitionHappening = TransitionMaster.GetInstance().IsTransitioning();
        return isInteracting || isBattleHappening || isTransitionHappening;
    }
}

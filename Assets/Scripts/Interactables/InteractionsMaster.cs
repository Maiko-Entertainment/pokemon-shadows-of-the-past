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
        events.Add(intEvent);
        isInteracting = true;
        if (events.Count == 1)
        {
            ExecuteNext(0);
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
        if (events.Count != 0)
        {
            print("Executing event: " + events[0]);
            events[0].Execute();
            events.RemoveAt(0);
        }
        else
        {
            isInteracting = false;
            if (!BattleMaster.GetInstance().GetCurrentBattle().IsBattleActive())
                UIPauseMenuMaster.GetInstance().ShowMenuOpener();
        }
    }

    public bool IsInteractionPlaying()
    {
        bool isBattleHappening = BattleMaster.GetInstance().GetCurrentBattle().IsBattleActive();
        bool isMenuOpen = UIPauseMenuMaster.GetInstance().IsMenuOpen();
        return isInteracting || isBattleHappening || isMenuOpen;
    }
}

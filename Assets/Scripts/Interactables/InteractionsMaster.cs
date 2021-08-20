﻿using System.Collections;
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
        events.Add(intEvent);
        isInteracting = true;
        if (events.Count == 1)
        {
            ExecuteNext(0);
        }
    }

    public void ExecuteNext(float time = 0f)
    {
        StartCoroutine(ExecuteAfterDelay(time));
    }

    private IEnumerator ExecuteAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        if (events.Count != 0)
        {
            events[0].Execute();
            events.RemoveAt(0);
        }
        else
        {
            isInteracting = false;
        }
    }

    public bool IsInteractionPlaying()
    {
        bool isBattleHappening = BattleMaster.GetInstance().GetCurrentBattle().IsBattleActive();
        return isInteracting || isBattleHappening;
    }
}

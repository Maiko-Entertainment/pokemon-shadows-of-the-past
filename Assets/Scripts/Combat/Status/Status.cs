﻿using Fungus;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Status
{
    public StatusEffectId effectId;
    public int minTurns = 1;
    public int addedRangeTurns = 3;
    public Flowchart message;
    public string onEndFlowchartBlock = "";

    protected List<BattleTrigger> battleTriggers = new List<BattleTrigger>();
    protected int turnsLeft = 0;
    protected bool stopEscape = false;

    public Status(Flowchart message)
    {
        this.message = message;
    }
    public virtual void Initiate()
    {
        // TO DO
        // Sets turns that the status will last and 
        // adds BattleTriggers to BattleEventManager
        turnsLeft = minTurns + Random.Range(0, addedRangeTurns);
    }

    public virtual bool IsOver()
    {
        return turnsLeft <= 0;
    }

    public virtual void PassTurn()
    {
        turnsLeft -= 1;
        if (IsOver())
        {
            Remove();
        }
    }

    public virtual void Remove()
    {
        if (onEndFlowchartBlock != "")
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(new BattleTriggerMessageData(message, onEndFlowchartBlock)));
        foreach (BattleTrigger bt in battleTriggers)
            BattleMaster.GetInstance()?.GetCurrentBattle()?.RemoveTrigger(bt);
        turnsLeft = 0;
    }
}

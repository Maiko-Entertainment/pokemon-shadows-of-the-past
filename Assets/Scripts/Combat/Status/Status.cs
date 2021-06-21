using Fungus;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public StatusEffectId effectId;
    public int minTurns = 1;
    public int addedRangeTurns = 3;

    protected List<BattleTrigger> battleTriggers = new List<BattleTrigger>();
    protected int turnsLeft = 0;

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
        // TO DO remove BattleTriggers from BattleEventManager
        foreach (BattleTrigger bt in battleTriggers)
            BattleMaster.GetInstance()?.GetCurrentBattle()?.RemoveTrigger(bt);
    }
}

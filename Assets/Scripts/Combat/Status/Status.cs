using Fungus;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Status
{
    public StatusEffectId effectId;
    public int minTurns = 1;
    public int addedRangeTurns = 3;
    public string onEndFlowchartBlock = "";
    public string onWarningFlowchartBlock = "";

    protected List<BattleTrigger> battleTriggers = new List<BattleTrigger>();
    protected List<BattleStatsGetter> statGetters = new List<BattleStatsGetter>();
    protected int turnsLeft = 0;
    protected bool stopEscape = false;

    public Status()
    {
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
        {
            Flowchart battleFlow = BattleAnimatorMaster.GetInstance().battleFlowchart;
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(new BattleTriggerMessageData(battleFlow, onEndFlowchartBlock)));
        }
        foreach (BattleTrigger bt in battleTriggers)
            BattleMaster.GetInstance()?.GetCurrentBattle()?.RemoveTrigger(bt);
        foreach (BattleStatsGetter sg in statGetters)
            BattleMaster.GetInstance()?.GetCurrentBattle()?.RemoveStatGetter(sg);
        turnsLeft = 0;
    }
}

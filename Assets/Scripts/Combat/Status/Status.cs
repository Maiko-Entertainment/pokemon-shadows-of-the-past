using Fungus;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Status
{
    public int minTurns = 1;
    public int addedRangeTurns = 3;
    public string gainStatusBlockName = "";
    public string onEndFlowchartBlock = "";
    public string onWarningFlowchartBlock = "";

    protected List<BattleTrigger> _battleTriggers = new List<BattleTrigger>();
    protected List<BattleStatsGetter> _statsGetters = new List<BattleStatsGetter>();
    protected List<IBattleAnimationConstant> constantAnimations = new List<IBattleAnimationConstant>();

    protected int turnsLeft = 0;
    protected int initialTurnsDuration = 0;
    protected bool stopEscape = false;
    protected Flowchart flowchartInstance = null;

    public Status(Flowchart flowchartInstance)
    {
        this.flowchartInstance = flowchartInstance;
    }

    public virtual void Initiate()
    {
        turnsLeft = minTurns + Random.Range(0, addedRangeTurns);
        initialTurnsDuration = turnsLeft;

        foreach (BattleTrigger bt in _battleTriggers)
        {
            BattleMaster.GetInstance()?
                .GetCurrentBattle()?.AddTrigger(
                    bt
                );
        }
        foreach (BattleStatsGetter sg in _statsGetters)
        {
            BattleMaster.GetInstance()?.GetCurrentBattle()?.AddStatGetter(sg);
        }
    }

    public virtual bool IsOver()
    {
        return turnsLeft <= 0;
    }

    public virtual void PassRound()
    {
        turnsLeft -= 1;
        if (IsOver())
        {
            Remove();
        }
    }

    public virtual void Remove()
    {
        if (flowchartInstance)
        {
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventDestroy(flowchartInstance.gameObject));
        }
        foreach (BattleTrigger bt in _battleTriggers)
            BattleMaster.GetInstance()?.GetCurrentBattle()?.RemoveTrigger(bt);
        foreach (BattleStatsGetter sg in _statsGetters)
        {
            BattleMaster.GetInstance()?.GetCurrentBattle()?.RemoveStatGetter(sg);
        }
        turnsLeft = 0;
    }

    public int GetTurnsPassed()
    {
        return initialTurnsDuration - turnsLeft;
    }

    public virtual void AddBattleTrigger(BattleTrigger bt)
    {
        _battleTriggers.Add(bt);
    }

    public virtual void AddConstantAnimation(IBattleAnimationConstant battleAnimationConstant)
    {
        constantAnimations.Add(battleAnimationConstant);
    }

    public List<IBattleAnimationConstant> GetConstantAnimations()
    {
        return constantAnimations;
    }
}

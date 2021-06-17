using System.Collections.Generic;
[System.Serializable]
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
    }

    public virtual bool IsOver()
    {
        return turnsLeft <= 0;
    }

    public virtual void Remove()
    {
        // TO DO remove BattleTriggers from BattleEventManager
    }
}

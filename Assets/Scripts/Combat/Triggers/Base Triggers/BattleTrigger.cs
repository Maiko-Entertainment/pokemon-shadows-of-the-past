﻿[System.Serializable]
public class BattleTrigger
{
    public BattleEventId eventId;
    public int maxTriggers = 99999999;
    public int turnsLeft = 9999999;
    public float priority = 0;

    protected int amountOfTimesTriggered = 0;

    public virtual bool Execute(BattleEvent battleEvent)
    {
        maxTriggers -= 1;
        amountOfTimesTriggered++;
        return true;
    }

    public virtual bool MeetsConditions(BattleEvent ev)
    {
        return maxTriggers > 0 && turnsLeft > 0;
    }
}

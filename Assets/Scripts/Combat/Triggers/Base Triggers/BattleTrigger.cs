[System.Serializable]
public class BattleTrigger
{
    public BattleEventId eventId;
    public int maxTriggers = 99999999;
    public int turnsLeft = 9999999;
    public virtual bool Execute(BattleEvent battleEvent) 
    {
        maxTriggers -= 1;
        return true;
    }
}

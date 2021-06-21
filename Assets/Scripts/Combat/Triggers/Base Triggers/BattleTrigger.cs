[System.Serializable]
public class BattleTrigger
{
    public BattleEventId eventId;

    public virtual bool Execute(BattleEvent battleEvent) 
    {
        return true;
    }
}

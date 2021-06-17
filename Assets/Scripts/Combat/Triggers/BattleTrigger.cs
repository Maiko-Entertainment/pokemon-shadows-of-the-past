[System.Serializable]
public class BattleTrigger
{
    public BattleEventId eventId;

    public virtual bool Execute(BattleEvent battleEvent) 
    {
        return true;
    }

    public override string ToString()
    {
        return eventId.ToString();
    }
}

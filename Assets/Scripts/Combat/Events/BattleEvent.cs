// Events are accumulated in a queue in the battle system
// The battle system will first try to execute the event, triggering BattleTriggers
// An event may be modified by a trigger or even delete it before it can be excecuted.
// All events are deleted after excecution
public class BattleEvent
{
    public BattleEventId eventId;

    public virtual void Execute()
    {

    }
}

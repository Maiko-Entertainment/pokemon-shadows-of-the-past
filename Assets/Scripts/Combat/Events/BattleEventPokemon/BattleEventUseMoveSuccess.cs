// Used for triggers that require knowing when a move was executed successfully.
public class BattleEventUseMoveSuccess : BattleEvent
{
    public BattleEventUseMove moveEvent;
    public BattleEventUseMoveSuccess(BattleEventUseMove moveEvent)
    {
        eventId = BattleEventId.pokemonUseMoveSuccess;
        this.moveEvent = moveEvent;
    }

    public override void Execute()
    {
        
    }
}

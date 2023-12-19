// Used for triggers that require knowing when a move was executed unsucesfully.
public class BattleEventUseMoveFail : BattleEvent
{
    public BattleEventUseMove moveEvent;
    public BattleEventUseMoveFail(BattleEventUseMove moveEvent)
    {
        eventId = BattleEventId.pokemonUseMoveFail;
        this.moveEvent = moveEvent;
    }

    public override void Execute()
    {
        
    }
}

public class BattleTriggerOnPokemonRoundEnd : BattleTriggerOnPokemon
{

    public BattleTriggerOnPokemonRoundEnd(PokemonBattleData pbd, bool deleteOnLeave = true) :
        base(pbd, deleteOnLeave)
    {
        eventId = BattleEventId.roundEnd;
    }

    public virtual bool Execute(BattleEventRoundEnd battleEvent)
    {
        return base.Execute(battleEvent);
    }
}

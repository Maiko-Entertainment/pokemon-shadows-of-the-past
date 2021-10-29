public class BattleTriggerOnPokemonEnter : BattleTriggerOnPokemon
{
    public BattleTriggerOnPokemonEnter(PokemonBattleData pokemon, bool deleteOnLeave) : base(pokemon, deleteOnLeave)
    {
        eventId = BattleEventId.pokemonEnter;
    }
    
    public virtual bool Execute(BattleEventEnterPokemon battleEvent)
    {
        return true;
    }
}

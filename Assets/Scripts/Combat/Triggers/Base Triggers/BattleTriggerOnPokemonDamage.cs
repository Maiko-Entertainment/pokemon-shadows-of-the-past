public class BattleTriggerOnPokemonDamage : BattleTriggerOnPokemon
{
    public BattleEventTakeDamageSuccess damageEvent;
    public BattleTriggerOnPokemonDamage(PokemonBattleData pokemon, bool deleteOnLeave = false) : base(pokemon, deleteOnLeave)
    {
        eventId = BattleEventId.pokemonTakeDamageSuccess;
    }

    public virtual bool Execute(BattleEventTakeDamageSuccess battleEvent)
    {
        return base.Execute(battleEvent);
    }
}

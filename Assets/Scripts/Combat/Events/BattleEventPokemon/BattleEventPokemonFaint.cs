public class BattleEventPokemonFaint : BattleEventPokemon
{
    public BattleEventTakeDamage eventCauser;
    public BattleEventPokemonFaint(BattleEventTakeDamage eventCauser) :
        base(eventCauser.pokemon)
    {
        eventId = BattleEventId.pokemonFaint;
        this.eventCauser = eventCauser;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance()?.AddEventPokemonFaintText(pokemon);
        BattleAnimatorMaster.GetInstance()?.AddEvent(
            new BattleAnimatorEventPlaySound(pokemon.GetCry(), 0.75f));
       BattleAnimatorMaster.GetInstance()?.AddEvent(
            new BattleAnimatorEventPokemonFaint(pokemon)
            );
        base.Execute();
    }
}

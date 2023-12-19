using Fungus;

public class BattleTriggerOnPokemonFaintDialogue : BattleTriggerOnPokemonFaint
{
    BattleTriggerMessageData messageData;
    public BattleTriggerOnPokemonFaintDialogue(PokemonBattleData pokemon, BattleTriggerMessageData messageData) :
        base(pokemon, true)
    {
        this.messageData = messageData;
    }

    public override bool TryToExecute(BattleEvent battleEvent)
    {
        BattleEventPokemonFaint castEvent = (BattleEventPokemonFaint)battleEvent;
        PokemonBattleData faintedPokemon = castEvent.pokemon;
        if (faintedPokemon == pokemon)
        {
            DamageSummary summary = castEvent.eventCauser.damageSummary;
            PokemonBattleData pkmn = summary.pokemonSource;
            if (pkmn != null)
                messageData.variables.Add("pokemonFainted", pkmn.GetName());
            messageData.variables.Add("pokemon", faintedPokemon.GetName());
            BattleAnimatorMaster.GetInstance().AddEvent(
                new BattleAnimatorEventNarrative(
                        messageData
                )
            );

        }
        return base.TryToExecute(battleEvent);
    }
}

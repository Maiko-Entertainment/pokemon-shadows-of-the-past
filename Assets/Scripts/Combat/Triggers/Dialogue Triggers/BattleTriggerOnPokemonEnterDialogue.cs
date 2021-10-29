public class BattleTriggerOnPokemonEnterDialogue : BattleTriggerOnPokemonEnter
{
    BattleTriggerMessageData messageData;
    public BattleTriggerOnPokemonEnterDialogue(PokemonBattleData pokemon, BattleTriggerMessageData messageData) :
        base(pokemon, true)
    {
        this.messageData = messageData;
    }

    public override bool Execute(BattleEventEnterPokemon battleEvent)
    {
        if (battleEvent.pokemon == pokemon)
        {
            messageData.variables.Add("pokemon", pokemon.GetName());
            BattleAnimatorMaster.GetInstance().AddEvent(
                new BattleAnimatorEventNarrative(
                        messageData
                )
            );

        }
        return base.Execute(battleEvent);
    }
}

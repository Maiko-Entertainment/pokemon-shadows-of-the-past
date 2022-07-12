public class BattleTriggerOnPokemonRoundEnd : BattleTriggerOnRoundEnd
{
    public PokemonBattleData pokemon;
    public bool deleteOnLeave = true;
    public BattleTriggerOnPokemonRoundEnd(PokemonBattleData pokemon, bool deleteOnLeave = true) :
        base()
    {
        this.deleteOnLeave = deleteOnLeave;
        this.pokemon = pokemon;
        if (deleteOnLeave)
        {
            BattleMaster.GetInstance().GetCurrentBattle()
            .AddTrigger(new BattleTriggerCleanUp(pokemon, this));
        }
    }

    public override bool Execute(BattleEventRoundEnd battleEvent)
    {
        return base.Execute(battleEvent);
    }
}

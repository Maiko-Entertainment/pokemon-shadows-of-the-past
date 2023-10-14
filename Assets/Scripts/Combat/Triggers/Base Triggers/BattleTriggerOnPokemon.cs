public class BattleTriggerOnPokemon : BattleTrigger
{
    public PokemonBattleData pokemon;

    public BattleTriggerOnPokemon(PokemonBattleData pokemon, bool deleteOnLeave)
    {
        this.pokemon = pokemon;
        if (deleteOnLeave)
        {
            BattleMaster.GetInstance().GetCurrentBattle()
            .AddTrigger(new BattleTriggerCleanUp(pokemon, this));
        }
    }

    public virtual bool Execute(BattleEventPokemon battleEvent)
    {
        return base.Execute(battleEvent);
    }

    public override string ToString()
    {
        return ""+pokemon?.pokemon?.GetName();
    }
}

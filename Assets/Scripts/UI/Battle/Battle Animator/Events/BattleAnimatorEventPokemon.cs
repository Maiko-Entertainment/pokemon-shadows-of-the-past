public class BattleAnimatorEventPokemon : BattleAnimatorEvent
{
    public PokemonBattleData pokemon = new PokemonBattleData();

    public BattleAnimatorEventPokemon(PokemonBattleData pokemon)
    {
        this.pokemon = pokemon;
    }

    public override string ToString()
    {
        return base.ToString() + " - " + pokemon.GetName();
    }
}

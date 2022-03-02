public class BattleAnimatorEventPokemon : BattleAnimatorEvent
{
    public PokemonBattleData pokemon;

    public BattleAnimatorEventPokemon(PokemonBattleData pokemon)
    {
        this.pokemon = pokemon.Copy();
    }

    public override string ToString()
    {
        return base.ToString() + " - " + pokemon.GetName();
    }
}

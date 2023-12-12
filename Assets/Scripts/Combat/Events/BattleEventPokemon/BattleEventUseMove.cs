
public class BattleEventUseMove : BattleEventPokemon
{
    public MoveData move;
    public UseMoveMods moveMods;

    public BattleEventUseMove(PokemonBattleData pokemon, MoveData move):
        base(pokemon)
    {
        eventId = BattleEventId.pokemonUseMove;
        this.move = move;
        moveMods = new UseMoveMods(move.GetMoveType());
    }

    public override void Execute()
    {
        if (!pokemon.IsFainted())
        {
            move.Execute(this);
        }
    }
}

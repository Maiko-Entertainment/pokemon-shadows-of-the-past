﻿
public class BattleEventUseMove : BattleEventPokemon
{
    public MoveData move;
    public UseMoveMods moveMods;

    public BattleEventUseMove(PokemonBattleData pokemon, MoveData move):
        base(pokemon)
    {
        eventId = BattleEventId.pokemonUseMove;
        this.move = move;
        moveMods = new UseMoveMods(move.typeId);
    }

    public override void Execute()
    {
        if (!pokemon.IsFainted())
        {
            BattleMaster.GetInstance().GetCurrentBattle().lastUsedMove = move;
            move.Execute(this);
        }
    }
}

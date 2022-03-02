using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnDesitionPokemonMove : BattleTurnDesitionPokemon
{
    public MoveEquipped move;

    public BattleTurnDesitionPokemonMove(
        MoveEquipped move, 
        PokemonBattleData pokemon, 
        BattleTeamId teamId) :
        base(pokemon, teamId)
    {
        this.move = move;
    }

    public override void Execute()
    {
        BattleMaster.GetInstance().GetCurrentBattle()?
            .AddMoveEvent(pokemon, move.move);
    }

    public override float GetTiebreakerPriority()
    {
        return base.GetTiebreakerPriority() + move.move.priority;
    }
}

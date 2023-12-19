using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnDesitionCheckForMove : BattleTriggerOnDesition
{
    public PokemonBattleData pokemon;
    public MoveData move;

    public delegate void OnExecute(BattleEventDestion battleEvent, PokemonBattleData pokemon, MoveData move);
    public event OnExecute onExecute;

    public delegate void OnNotMove(BattleEventDestion battleEvent, PokemonBattleData pokemon, MoveData move);
    public event OnNotMove onNotUseMove;
    public BattleTriggerOnDesitionCheckForMove(BattleTeamId teamId, PokemonBattleData pokemon, MoveData move) : base(teamId)
    {
        this.pokemon = pokemon;
        this.move = move;
    }

    public override bool Execute(BattleEventDestion battleEvent)
    {
        PokemonBattleData activePokemon = BattleMaster.GetInstance().GetCurrentBattle().GetTeamActivePokemon(teamId);
        if (teamId == battleEvent.desition.team)
        {
            if (battleEvent.desition is not BattleTurnDesitionPokemonMove)
            {
                onNotUseMove?.Invoke(battleEvent, pokemon, move);
            }
            else
            {
                BattleTurnDesitionPokemonMove moveDestion = battleEvent.desition as BattleTurnDesitionPokemonMove;
                if (moveDestion.pokemon == pokemon)
                {
                    if (moveDestion.move.move.GetId() != moveDestion.move.move.GetId())
                    {
                        onNotUseMove?.Invoke(battleEvent, pokemon, move);
                    }
                    else
                    {
                        onExecute?.Invoke(battleEvent, pokemon, move);
                    }
                }
            }
        }
        return base.Execute(battleEvent);
    }
}

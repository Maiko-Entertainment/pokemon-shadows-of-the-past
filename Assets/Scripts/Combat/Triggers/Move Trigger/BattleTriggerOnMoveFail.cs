using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnMoveFail : BattleTriggerOnPokemon
{
    public MoveData move;

    public delegate void OnExecute(BattleEventUseMoveFail move);
    public OnExecute onExecute;

    public BattleTriggerOnMoveFail(PokemonBattleData pokemon): base (pokemon, true)
    {
        eventId = BattleEventId.pokemonUseMoveFail;
    }
    public BattleTriggerOnMoveFail(PokemonBattleData pokemon, MoveData move) : base(pokemon, true)
    {
        eventId = BattleEventId.pokemonUseMoveFail;
        this.move = move;
    }

    public override bool Execute(BattleEvent battleEvent)
    {
        BattleEventUseMoveFail be = battleEvent as BattleEventUseMoveFail;
        if (be != null)
        {
            onExecute?.Invoke(battleEvent as BattleEventUseMoveFail);
        }
        return base.Execute(battleEvent);
    }

    public override bool MeetsConditions(BattleEvent ev)
    {
        BattleEventUseMoveFail be = ev as BattleEventUseMoveFail;
        if (be != null)
        {
            BattleEventUseMove moveEvent = be.moveEvent;
            if (pokemon.battleId == moveEvent.pokemon.battleId)
            {
                if (move != null)
                {
                    if (moveEvent.move.GetId() == move.GetId())
                        return base.MeetsConditions(ev);
                    else return false;
                }
                else
                {
                    return base.MeetsConditions(ev);
                }
            }
            return false;
        }
        return false;
    }
}

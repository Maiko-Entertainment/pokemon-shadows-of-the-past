using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventPokemonMove : BattleAnimatorEventPokemon
{
    protected BattleEventUseMove moveEvent;
    public BattleAnimatorEventPokemonMove(BattleEventUseMove battleEvent) : base(battleEvent.pokemon)
    {
        moveEvent = battleEvent;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance().ExecuteMoveFlowchart(moveEvent);
        base.Execute();
    }
}

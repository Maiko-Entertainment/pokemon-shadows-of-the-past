using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventMoveMiss : BattleAnimatorEventPokemonMoveFlowchart
{
    public BattleAnimatorEventMoveMiss(BattleEventUseMove be) : base(be)
    {

    }
    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance().ExecuteMissMoveFlowchart(moveEvent);
    }

    public override string ToString()
    {
        return base.ToString() + " - Miss: " + moveEvent.move.moveName;
    }
}

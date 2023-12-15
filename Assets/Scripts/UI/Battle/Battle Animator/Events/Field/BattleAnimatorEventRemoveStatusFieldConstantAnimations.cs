using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventRemoveStatusFieldConstantAnimations : BattleAnimatorEvent
{
    public StatusField status;
    public BattleAnimatorEventRemoveStatusFieldConstantAnimations(StatusField status) :
        base()
    {
        eventType = BattleAnimatorEventType.PokemonMoveAnim;
        this.status = status;
    }
    public override void Execute()
    {
        float highestTime = 0f;
        foreach(BattleAnimationConstantField sfa in status.GetConstantAnimations())
        {
            highestTime = Mathf.Max(sfa.EndTime(), highestTime);
            sfa.End();
        }
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim(highestTime);
    }
}

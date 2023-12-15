using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventConstantFieldAnim : BattleAnimatorEvent
{
    public BattleAnimationConstantField battleAnimation;
    public StatusField status;
    public BattleAnimatorEventConstantFieldAnim(StatusField status, BattleAnimationConstantField animation) :
        base()
    {
        battleAnimation = animation;
        this.status = status;
    }
    public override void Execute()
    {
        BattleAnimationConstantField anim = GameObject.Instantiate(battleAnimation);
        anim.Execute();
        status.AddConstantAnimation(anim);
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim(anim.duration);
    }
}

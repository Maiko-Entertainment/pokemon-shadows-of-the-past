using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerRoundEndAnimations : BattleTriggerOnRoundEnd
{
    public List<BattleAnimationField> animations;
    public BattleTriggerRoundEndAnimations(List<BattleAnimationField> animations) : base()
    {
        this.animations = animations;
    }

    public override bool Execute(BattleEventRoundEnd roundEnd)
    {
        foreach(BattleAnimationField field in animations)
        {
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventAnimation(field));
        }
        return base.Execute(roundEnd);
    }
}

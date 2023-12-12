using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fields have a list of animations
public class BattleAnimationField : BattleAnimation
{

    // Execute lets the animator know that the animation has begun
    public override BattleAnimation Execute()
    {
        base.Execute();
        Transform parent = BattleAnimatorMaster.GetInstance().transform;
        transform.position = parent.position;
        return this;
    }

}

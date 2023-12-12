using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationConstantField: BattleAnimation, IBattleAnimationConstant
{
    public float endTime = 1f;

    public override BattleAnimation Execute()
    {
        Transform parent = BattleAnimatorMaster.GetInstance().transform;
        transform.position = parent.position;
        return this;
    }

    public float End()
    {
        Destroy(gameObject, EndTime());
        if (animator != null)
        {
            animator.SetTrigger("End");
        }
        return endTime;
    }

    public virtual float EndTime()
    {
        return endTime;
    }
}

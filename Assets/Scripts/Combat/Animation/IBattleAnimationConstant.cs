using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public interface IBattleAnimationConstant: IBattleAnimation
{
    public float End();

    public float EndTime();
}

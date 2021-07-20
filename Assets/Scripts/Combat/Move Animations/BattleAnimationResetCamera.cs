using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationResetCamera : BattleAnimation
{
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        destroyAfter = BattleAnimatorMaster.GetInstance().HandleCameraReset();
        return base.Execute(user, target);
    }
}

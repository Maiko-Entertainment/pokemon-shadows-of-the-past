using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationFromUserToTarget : BattleAnimation
{
    protected float timePassed = 0;
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        return base.Execute(user, target);
    }

    private void FixedUpdate()
    {
        Vector3 origin = BattleAnimatorMaster.GetInstance().GetPokemonPosition(user);
        Vector3 destiny = BattleAnimatorMaster.GetInstance().GetPokemonPosition(target);
        transform.position = Vector3.Lerp(origin, destiny, timePassed / destroyAfter);
        timePassed += Time.fixedDeltaTime;
    }
}

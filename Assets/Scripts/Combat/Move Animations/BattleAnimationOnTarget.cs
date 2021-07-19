using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationOnTarget : BattleAnimation
{
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        transform.position = BattleAnimatorMaster.GetInstance().GetPokemonPosition(target);
        return base.Execute(user, target);
    }

    public void Update()
    {
        
    }
}

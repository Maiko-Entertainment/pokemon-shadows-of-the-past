using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationOnUser : BattleAnimation
{
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        transform.localPosition = BattleAnimatorMaster.GetInstance().GetPokemonPosition(user);
        return base.Execute(user, target);
    }
}

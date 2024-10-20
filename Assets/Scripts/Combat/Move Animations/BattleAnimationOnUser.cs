﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationOnUser : BattleAnimation
{
    public bool isChildrenOfUser = false;
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        if (isChildrenOfUser)
        {
            transform.parent = BattleAnimatorMaster.GetInstance().GetPokemonTeamTransform(user);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.localPosition = BattleAnimatorMaster.GetInstance().GetPokemonPosition(user);
        }
        return base.Execute(user, target);
    }
}

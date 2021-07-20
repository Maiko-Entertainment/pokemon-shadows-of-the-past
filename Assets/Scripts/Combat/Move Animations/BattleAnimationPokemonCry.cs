using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationPokemonCry : BattleAnimation
{
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        AudioMaster.GetInstance().PlaySfx(user.GetCry());
        return base.Execute(user, target);
    }
}

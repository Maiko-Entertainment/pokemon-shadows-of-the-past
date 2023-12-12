using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationResetCamera : BattleAnimationPokemon
{
    public override BattleAnimationPokemon Execute(PokemonBattleData user, PokemonBattleData target)
    {
        destroyAfter = BattleAnimatorMaster.GetInstance().HandleCameraReset();
        return base.Execute(user, target);
    }
}

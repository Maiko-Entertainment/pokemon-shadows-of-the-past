using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorShrinkTarget : BattleAnimation
{
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        Shrink(target);
        return base.Execute(user, target);
    }

    public void Shrink(PokemonBattleData target)
    {
        Transform pokemonTransform = BattleAnimatorMaster.GetInstance().GetPokemonTeamTransform(target);
        TransitionSize transition = pokemonTransform.gameObject.AddComponent<TransitionSize>();
        transition.initialSize = pokemonTransform.localScale;
        transition.finalSize = Vector3.zero;
        transition.speed = 1.2f;
        transition.FadeIn();
    }
}

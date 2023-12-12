using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationReturnDefaultSize : BattleAnimationPokemon
{
    public override BattleAnimationPokemon Execute(PokemonBattleData user, PokemonBattleData target)
    {
        Reset(target);
        return base.Execute(user, target);
    }

    public void Reset(PokemonBattleData target)
    {
        Transform pokemonTransform = BattleAnimatorMaster.GetInstance().GetPokemonTeamTransform(target);
        pokemonTransform.localPosition = Vector3.zero;
        TransitionSize transition = pokemonTransform.gameObject.AddComponent<TransitionSize>();
        transition.initialSize = pokemonTransform.localScale;
        transition.finalSize = Vector3.one;
        transition.speed = 1 / (destroyAfter * 0.9f);
        transition.FadeIn();
    }
}

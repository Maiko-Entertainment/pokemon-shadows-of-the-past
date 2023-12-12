using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorCatchPokemon : BattleAnimationPokemon
{
    public float shrinkTime = 1f;
    public float moveUpTime = 1f;
    public override BattleAnimationPokemon Execute(PokemonBattleData user, PokemonBattleData target)
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
        transition.speed = 1 / shrinkTime;
        transition.FadeIn();
        TransitionMoveGameObject transitionMove = pokemonTransform.gameObject.AddComponent<TransitionMoveGameObject>();
        transitionMove.finalPosition = Vector2.up * 2f;
        transitionMove.FadeIn();
        transitionMove.speed = 1f / moveUpTime;
        Destroy(transitionMove, moveUpTime);
    }
}

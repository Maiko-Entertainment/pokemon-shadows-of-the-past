using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationStompTarget : BattleAnimation
{
    public Vector3 finalChange;
    public bool targetUserInstead = false;
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        Stomp(targetUserInstead ? user : target);
        return base.Execute(user, target);
    }
    public void Stomp(PokemonBattleData target)
    {
        Transform pokemonTransform = BattleAnimatorMaster.GetInstance().GetPokemonTeamTransform(target);
        TransitionSize transition = pokemonTransform.gameObject.AddComponent<TransitionSize>();
        transition.initialSize = pokemonTransform.localScale;
        transition.finalSize = finalChange;
        transition.speed = 2 / destroyAfter;
        transition.FadeIn();
        Destroy(transition, destroyAfter);
        StartCoroutine(ReturnToPrevious(pokemonTransform));
    }

    IEnumerator ReturnToPrevious(Transform pokemonTransform)
    {
        yield return new WaitForSeconds(destroyAfter / 2f);
        TransitionSize transition = pokemonTransform.gameObject.AddComponent<TransitionSize>();
        transition.initialSize = finalChange;
        transition.finalSize = Vector3.one;
        transition.speed = 2 / destroyAfter;
        transition.FadeIn();
        // Destroy(transition, destroyAfter);
    }
}

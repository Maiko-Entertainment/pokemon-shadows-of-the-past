using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventPokemonFaint : BattleAnimatorEventPokemon
{
    public BattleAnimatorEventPokemonFaint(PokemonBattleData pokemon):
        base(pokemon)
    {

    }

    public override void Execute()
    {
        base.Execute();
        Transform pokemonTransform = BattleAnimatorMaster.GetInstance().GetPokemonTeamTransform(pokemon);
        BattleAnimatorMaster.GetInstance().HidePokemonInfo(BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(pokemon));
        TransitionSize transition = pokemonTransform.gameObject.AddComponent<TransitionSize>();
        transition.initialSize = pokemonTransform.localScale;
        transition.finalSize = Vector3.zero;
        transition.speed = 1.2f;
        transition.FadeIn();
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim(1.2f);
    }
}

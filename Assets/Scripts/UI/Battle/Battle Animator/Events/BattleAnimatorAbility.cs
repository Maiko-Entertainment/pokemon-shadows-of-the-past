using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorAbility : BattleAnimatorEvent
{
    PokemonBattleData pokemon;
    public BattleAnimatorAbility(PokemonBattleData pokemon)
    {
        this.pokemon = pokemon;
        eventType = BattleAnimatorEventType.PokemonAbilityAnim;
    }

    public override void Execute()
    {
        float delay = BattleAnimatorMaster.GetInstance().ShowPokemonAbility(pokemon);
        base.Execute();
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim(delay);
    }
}

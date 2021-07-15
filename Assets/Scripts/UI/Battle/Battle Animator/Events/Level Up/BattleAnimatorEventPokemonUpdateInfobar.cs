using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventPokemonUpdateInfobar : BattleAnimatorEventPokemon
{
    public BattleAnimatorEventPokemonUpdateInfobar(PokemonBattleData pokemon):
        base(pokemon)
    {

    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance()?.UpdatePokemonInfo(pokemon);
        BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim(0.2f);
        base.Execute();
    }
}

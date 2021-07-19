using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventPokemonEnterAnim : BattleAnimatorEventPokemon
{
    public BattleAnimatorEventPokemonEnterAnim(PokemonBattleData pokemon):
        base(pokemon)
    {

    }

    public override void Execute()
    {
        base.Execute();
        BattleTeamId id = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(pokemon);
        BattleAnimatorMaster.GetInstance().SetTeamPokemon(pokemon, id);
        BattleAnimatorMaster.GetInstance().HandlePokemonEnterAnim(pokemon);
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim(1f);
    }
}

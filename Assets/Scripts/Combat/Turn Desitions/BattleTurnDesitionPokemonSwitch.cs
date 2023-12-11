using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnDesitionPokemonSwitch : BattleTurnDesitionPokemon
{
    public BattleTurnDesitionPokemonSwitch(
        PokemonBattleData pokemon,
        BattleTeamId teamId) :
        base(pokemon, teamId)
    {
        priority = 5;
    }

    public override void Execute()
    {
        base.Execute();
        BattleMaster.GetInstance().GetCurrentBattle()?
            .AddSwitchInPokemonEvent(pokemon, true);
    }
}

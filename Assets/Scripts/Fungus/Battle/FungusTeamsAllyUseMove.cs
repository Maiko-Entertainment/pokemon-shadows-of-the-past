using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Combat",
    "Team's Ally Use Move",
    "Uses a move against a team."
)]
public class FungusTeamsAllyUseMove : Command
{
    public MoveData move;
    public int allyIndex;
    public BattleTeamId teamId;

    public override void OnEnter()
    {
        BattleTeamData teamData = BattleMaster.GetInstance().GetCurrentBattle().GetTeamData(teamId);
        PokemonBattleData pokemonAlly = teamData.allyPokemon[allyIndex];
        if (pokemonAlly != null)
        {
            BattleMaster.GetInstance().GetCurrentBattle()?.AddMoveEvent(
                pokemonAlly,
                move
            );
            BattleMaster.GetInstance().GetCurrentBattle().eventManager.ResolveAllEventTriggers();
            BattleMaster.GetInstance().GetCurrentBattle().CheckForFainted();
        }
        Continue();
    }

    public override Color GetButtonColor()
    {
        return Color.red;
    }
}

using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Combat",
    "End Battle",
    "End the current battle."
)]
public class FungusEndBattle : Command
{
    public BattleTeamId winnerTeam;

    public override void OnEnter()
    {
        BattleAnimatorMaster.GetInstance().ClearEvents();
        BattleMaster.GetInstance()?.GetCurrentBattle()?.HandleBattleEnd(winnerTeam, true);
        Continue();
    }
}

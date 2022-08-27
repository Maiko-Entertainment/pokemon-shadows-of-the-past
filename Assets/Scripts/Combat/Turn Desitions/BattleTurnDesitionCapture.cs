using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnDesitionCapture : BattleTurnDesition
{
    public ItemDataPokeball pokeball;
    public BattleTurnDesitionCapture(ItemDataPokeball pokeball, BattleTeamId teamId):
        base(teamId)
    {
        this.pokeball = pokeball;
        priority = 7;
    }

    public override void Execute()
    {
        BattleMaster.GetInstance()?.GetCurrentBattle().AddEvent(new BattleEventUsePokeball(pokeball));
    }
}

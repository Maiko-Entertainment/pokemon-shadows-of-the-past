﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnDesitionCapture : BattleTurnDesition
{
    public ItemDataPokeball pokeball;
    public BattleTurnDesitionCapture(ItemDataPokeball pokeball, BattleTeamId teamId):
        base(teamId)
    {
        this.pokeball = pokeball;
    }

    public override void Execute()
    {
        pokeball.UseInBattle();
    }
}

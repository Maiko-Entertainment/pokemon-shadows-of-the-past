﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnDesitionPokemon : BattleTurnDesition
{
    public PokemonBattleData pokemon;
    public BattleTurnDesitionPokemon(PokemonBattleData pokemon, BattleTeamId teamId) : base(teamId)
    {
        this.pokemon = pokemon;
    }

    public override float GetTiebreakerPriority()
    {
        return base.GetTiebreakerPriority() + BattleMaster.GetInstance().GetCurrentBattle().GetPokemonStats(pokemon).speed / 1000000f;
    }
}

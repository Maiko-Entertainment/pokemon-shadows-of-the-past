using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnDesitionPokemon : BattleTurnDesition
{
    public PokemonBattleData pokemon;
    public BattleTurnDesitionPokemon(PokemonBattleData pokemon, BattleTeamId teamId) : base(teamId)
    {
        this.pokemon = pokemon;
    }

}

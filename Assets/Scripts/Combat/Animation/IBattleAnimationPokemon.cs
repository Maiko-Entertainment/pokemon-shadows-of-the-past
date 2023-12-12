using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleAnimationPokemon
{
    public BattleAnimationPokemon Execute(PokemonBattleData user, PokemonBattleData target);
}

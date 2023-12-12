using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationPokemonCry : BattleAnimationPokemon
{
    public float customPitch = 1f;
    public override BattleAnimationPokemon Execute(PokemonBattleData user, PokemonBattleData target)
    {
        AudioMaster.GetInstance().PlaySfx(user.GetCry(), customPitch);
        return base.Execute(user, target);
    }
}

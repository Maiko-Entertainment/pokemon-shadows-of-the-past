using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationSoundEffect : BattleAnimationPokemon
{
    public AudioClip clip;

    public override BattleAnimationPokemon Execute(PokemonBattleData user, PokemonBattleData target)
    {
        AudioMaster.GetInstance().PlaySfx(clip);
        return base.Execute(user, target);
    }
}

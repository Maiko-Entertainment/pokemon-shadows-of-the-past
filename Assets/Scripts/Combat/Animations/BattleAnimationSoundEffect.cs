using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationSoundEffect : BattleAnimation
{
    public AudioClip clip;

    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        AudioMaster.GetInstance().PlaySfx(clip);
        return base.Execute(user, target);
    }
}

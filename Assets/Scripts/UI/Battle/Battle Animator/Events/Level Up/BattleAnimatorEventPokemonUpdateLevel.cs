using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventPokemonUpdateLevel : BattleAnimatorEventPokemon
{
    public int level;

    public BattleAnimatorEventPokemonUpdateLevel(PokemonBattleData pokemon, int level):
        base(pokemon)
    {
        this.level = level;
        eventType = BattleAnimatorEventType.PokemonInfoChange;
    }

    public override void Execute()
    {
        AudioClip levelUpClip = BattleAnimatorMaster.GetInstance().levelUpClip;
        AudioMaster.GetInstance()?.PlaySfx(levelUpClip);
        BattleAnimatorMaster.GetInstance().UpdatePokemonLevel(pokemon, level);
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim(0.5f);
    }

    public override string ToString()
    {
        return base.ToString() + " - Update Level - "+level;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerRoundEndAnimations : BattleTriggerOnPokemonRoundEnd
{
    PokemonBattleData target;
    public List<BattleAnimation> animations;
    public BattleTriggerRoundEndAnimations(PokemonBattleData user, PokemonBattleData target, List<BattleAnimation> animations):
        base(user)
    {
        this.animations = animations;
        this.target = target;
    }
    public override bool Execute(BattleEventRoundEnd battleEvent)
    {
        if (!target.IsFainted() && !pokemon.IsFainted())
        {
            foreach (BattleAnimation anim in animations)
            {
                BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPokemonMoveAnimation(pokemon, target, anim));
            }
        }
        return base.Execute(battleEvent);
    }
}

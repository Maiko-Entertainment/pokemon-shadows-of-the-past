using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventAnimation : BattleEventPokemon
{
    public PokemonBattleData target;
    public BattleAnimationPokemon animation;
    public BattleEventAnimation (PokemonBattleData user, PokemonBattleData target, BattleAnimationPokemon animation) : base(user)
    {
        this.target = target;
        this.animation = animation;
        eventId = BattleEventId.animation;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventAnimation(pokemon, target, animation));
        base.Execute();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventAnimation : BattleEventPokemon
{
    public PokemonBattleData target;
    public BattleAnimation animation;
    public BattleEventAnimation (PokemonBattleData user, PokemonBattleData target, BattleAnimation animation) : base(user)
    {
        this.target = target;
        this.animation = animation;
        eventId = BattleEventId.animation;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPokemonMoveAnimation(pokemon, target, animation));
        base.Execute();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonAbility : BattleEventPokemon
{
    public int animationPriority = 0;

    public BattleEventPokemonAbility(PokemonBattleData pokemon): base(pokemon)
    {
        eventId = BattleEventId.pokemonAbilityUse;
    }

    public override void Execute()
    {
        base.Execute();
        BattleAnimatorAbility aa = new BattleAnimatorAbility(pokemon);
        aa.priority = animationPriority;
        BattleAnimatorMaster.GetInstance().AddEvent(aa);
    }
}

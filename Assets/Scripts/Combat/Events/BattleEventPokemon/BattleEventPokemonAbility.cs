using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonAbility : BattleEventPokemon
{
    public BattleEventPokemonAbility(PokemonBattleData pokemon): base(pokemon)
    {
        eventId = BattleEventId.pokemonAbilityUse;
    }

    public override void Execute()
    {
        base.Execute();
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorAbility(pokemon));
    }
}

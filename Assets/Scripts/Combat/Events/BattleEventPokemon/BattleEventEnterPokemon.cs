﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventEnterPokemon : BattleEventPokemon
{
    public BattleEventEnterPokemon(PokemonBattleData pokemon) :
         base(pokemon)
    {
        eventId = BattleEventId.pokemonEnter;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance().AddEventPokemonEnterText(pokemon);
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventEnterPokemon(pokemon));
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPokemonEnterAnim(pokemon));
        base.Execute();
    }
}

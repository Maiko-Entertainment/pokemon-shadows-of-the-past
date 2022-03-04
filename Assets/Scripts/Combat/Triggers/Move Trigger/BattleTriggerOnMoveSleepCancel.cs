using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnMoveSleepCancel : BattleTriggerOnPokemonMove
{

    public BattleTriggerOnMoveSleepCancel(PokemonBattleData pokemon, UseMoveMods mods) : base(pokemon, mods, true)
    {
        eventId = BattleEventId.pokemonUseMove;
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {

        if (pokemon == battleEvent.pokemon)
        {
            return false;
        }
        return base.Execute(battleEvent);
    }
}
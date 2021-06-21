using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonFaint : BattleTriggerOnPokemon
{
    public BattleTriggerOnPokemonFaint(BattleEventPokemonFaint battleEvent, bool deleteOnLeave = true) :
        base(battleEvent.pokemon, deleteOnLeave)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonFaint : BattleEventPokemon
{
    public BattleEventTakeDamage eventCauser;
    public BattleEventPokemonFaint(BattleEventTakeDamage eventCauser) :
        base(eventCauser.pokemon)
    {
        this.eventCauser = eventCauser;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventTakeDamageSuccess : BattleEventPokemon
{
    public BattleEventTakeDamage damageEvent;

    public BattleEventTakeDamageSuccess(BattleEventTakeDamage damageEvent): base(damageEvent.pokemon)
    {
        eventId = BattleEventId.pokemonTakeDamageSuccess;
        this.damageEvent = damageEvent;
    }
    public override void Execute()
    {
        base.Execute();
    }
}

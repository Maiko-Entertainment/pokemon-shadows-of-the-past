using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonStatusAddSuccess : BattleEvent
{
    public BattleEventPokemonStatusAdd statusEvent;
    public BattleEventPokemonStatusAddSuccess(BattleEventPokemonStatusAdd statusEvent)
    {
        eventId = BattleEventId.pokemonAddStatusSuccess;
        this.statusEvent = statusEvent;
    }

    public virtual void Execute(BattleEventPokemonStatusAdd statusEvent)
    {
        base.Execute();
    }
}

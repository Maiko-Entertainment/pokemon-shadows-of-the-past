using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonFaint : BattleEventPokemon
{
    public BattleEventTakeDamage eventCauser;
    public BattleEventPokemonFaint(BattleEventTakeDamage eventCauser) :
        base(eventCauser.pokemon)
    {
        eventId = BattleEventId.pokemonFaint;
        this.eventCauser = eventCauser;
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance()?.AddEventPokemonFaintText(pokemon);
        base.Execute();
    }
}

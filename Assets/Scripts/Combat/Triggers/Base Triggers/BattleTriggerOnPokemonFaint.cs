using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonFaint : BattleTriggerOnPokemon
{
    public BattleTriggerOnPokemonFaint(PokemonBattleData pokemon, bool deleteOnLeave = true) :
        base(pokemon, deleteOnLeave)
    {
        eventId = BattleEventId.pokemonFaint;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonChangeStats : BattleTriggerOnPokemon
{
    public BattleTriggerOnPokemonChangeStats(PokemonBattleData pokemon, bool deleteOnLeave) : base(pokemon, deleteOnLeave)
    { }

    public virtual bool Execute(BattleEventPokemonChangeStat battleEvent)
    {
        return true;
    }
}

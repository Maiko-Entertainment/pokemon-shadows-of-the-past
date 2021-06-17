using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonEnter : BattleTriggerOnPokemon
{
    public BattleTriggerOnPokemonEnter(PokemonBattleData pokemon, bool deleteOnLeave) : base(pokemon, deleteOnLeave)
    {}
    
    public virtual bool Execute(BattleEventEnterPokemon battleEvent)
    {
        return true;
    }
}

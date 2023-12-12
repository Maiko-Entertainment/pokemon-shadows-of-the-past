using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerConditionData : ScriptableObject
{
    public bool invertCondition = false;
    public virtual bool MeetsConditions(PokemonBattleData pokemon, MoveData moveUsed)
    {
        return MeetsConditions(pokemon);
    }

    // Check for Pokemon Conditions
    public virtual bool MeetsConditions(PokemonBattleData pokemon)
    {
        return true;
    }
}

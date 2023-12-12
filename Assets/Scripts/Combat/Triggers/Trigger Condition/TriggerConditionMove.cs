using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TriggerConditionMove : TriggerCondition
{
    public bool focusOnEnemiesTargetingPokemonInstead = false;

    public override bool MeetsConditions(PokemonBattleData pokemon, MoveData moveUsed)
    {
        return base.MeetsConditions(pokemon,moveUsed) && MeetsConditions(pokemon);
    }

    public override TriggerCondition Clone()
    {
        TriggerConditionMove clone = new TriggerConditionMove();
        clone.conditionsData = conditionsData;
        clone.focusOnEnemiesTargetingPokemonInstead = focusOnEnemiesTargetingPokemonInstead;
        return clone;
    }
}

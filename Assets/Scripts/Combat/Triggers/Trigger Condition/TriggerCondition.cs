using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TriggerCondition
{
    public List<TriggerConditionData> conditionsData = new List<TriggerConditionData>();

    public virtual bool MeetsConditions(PokemonBattleData pokemon, MoveData moveUsed)
    {
        foreach(TriggerConditionData triggerConditionData in conditionsData)
        {
            if (!triggerConditionData.MeetsConditions(pokemon, moveUsed)) return false;
        }
        return true;
    }

    // Check for Pokemon Conditions
    public virtual bool MeetsConditions(PokemonBattleData pokemon)
    {
        foreach (TriggerConditionData triggerConditionData in conditionsData)
        {
            if (!triggerConditionData.MeetsConditions(pokemon)) return false;
        }
        return true;
    }

    public virtual TriggerCondition Clone()
    {
        TriggerCondition clone = new TriggerCondition();
        clone.conditionsData = new List<TriggerConditionData>(conditionsData);
        return clone;
    }

    public List<TriggerConditionData> ConditionDatas { get { return conditionsData; } }
}

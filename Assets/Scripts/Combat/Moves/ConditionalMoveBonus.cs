using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ConditionalMoveBonus
{
    public List<TriggerConditionData> triggerConditionData = new List<TriggerConditionData>();
    // Ignore target for weather only conditions
    public MoveTarget target;
    public float accuracyBonusAdd = 0f;
    public float powerMultiplier = 1f;
    public float healMultiplier = 1f;

    public bool MeetsConditions(PokemonBattleData pokemon)
    {
        foreach(TriggerConditionData condition in triggerConditionData)
        {
            if (!condition.MeetsConditions(pokemon)) return false;
        }
        return true;
    }
}

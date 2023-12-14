using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Trigger Condition/At least one Status Field")]
public class TriggerConditionStatusFieldActive : TriggerConditionData
{
    public List<StatusFieldData> possibleFields = new List<StatusFieldData>();

    public override bool MeetsConditions(PokemonBattleData pokemon)
    {
        List<StatusField> activeFields = BattleMaster.GetInstance().GetCurrentBattle().GetStatusFields();
        foreach (StatusField field in activeFields)
        {
            if (possibleFields.Contains(field.FieldData))
            {
                return !invertCondition;
            }
        }
        return invertCondition;
    }
}

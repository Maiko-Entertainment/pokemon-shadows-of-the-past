using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Field Status")]
public class StatusFieldData : StatusData
{
    public FieldCategoryData fieldCategory;
    [Header("Trigger Conditions for move modifiers")]
    public List<MoveTriggerCondition> triggerConditionsMove = new List<MoveTriggerCondition>();
    // TODO: Add data to empower certain move types and increase stats

    public virtual StatusField CreateStatusInstance(PokemonBattleData pokemon)
    {
        Flowchart flowchartInstance = Instantiate(flowchart);
        StatusField se = new StatusField(this, flowchartInstance);
        se.minTurns = minTurnDuration;
        se.addedRangeTurns = extraTurnRange;

        // Generic move use logic
        foreach (MoveTriggerCondition moveTriggerCon in triggerConditionsMove)
        {
            BattleTriggerOnPokemonMove moveTrigger = new BattleTriggerOnPokemonMove(
                pokemon,
                moveTriggerCon.moveMods.Clone(),
                moveTriggerCon.triggerCondition.Clone() as TriggerConditionMove
            );
            moveTrigger.cancelMoveInstead = moveTriggerCon.cancelMoveInstead;
            se.AddBattleTrigger(moveTrigger);
        }
        return se;
    }
}

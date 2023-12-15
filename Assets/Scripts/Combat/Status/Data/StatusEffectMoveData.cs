using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Status/Pokemon/Require Move")]
public class StatusEffectMoveData : StatusEffectData
{
    [Header("Trigger Conditions for moves")]
    public List<MoveTriggerCondition> triggerConditionsMove = new List<MoveTriggerCondition>();
    [Header("Use this for moves that get stronger when repeated")]
    public float onRepeatMovePowerMultiplier = 1f;
    public bool endStatusIfMoveFailsToRepeat = false;

    public override StatusEffect CreateStatusInstance(PokemonBattleData pokemon, MoveData creator)
    {
        StatusEffect statusInstance = base.CreateStatusInstance(pokemon, creator);
        // Generic move use logic
        foreach (MoveTriggerCondition moveTriggerCon in triggerConditionsMove)
        {
            BattleTriggerOnPokemonMove moveTrigger = new BattleTriggerOnPokemonMove(
                pokemon,
                moveTriggerCon.moveMods.Clone(),
                moveTriggerCon.triggerCondition.Clone() as TriggerConditionMove
            );
            moveTrigger.cancelMoveInstead = moveTriggerCon.cancelMoveInstead;
            statusInstance.AddBattleTrigger(moveTrigger);
        }

        // Logic for when the move that created this status is used again
        BattleTriggerOnPokemonMoveRepeatMove repeatMove = new BattleTriggerOnPokemonMoveRepeatMove(pokemon, new UseMoveMods(null), creator);
        repeatMove.statusSource = statusInstance;
        repeatMove.endStatusOnMoveFailToRepeat = endStatusIfMoveFailsToRepeat;
        repeatMove.triggerPowerMultiplierAcum = onRepeatMovePowerMultiplier;
        statusInstance.AddBattleTrigger(repeatMove);
        return statusInstance;
    }
}
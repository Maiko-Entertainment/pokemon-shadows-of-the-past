using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Field Status")]
public class StatusFieldData : StatusData
{
    public FieldCategoryData fieldCategory;

    public List<BattleAnimationField> triggerAnimations = new List<BattleAnimationField>();
    public List<BattleAnimationConstantField> constantAnimations = new List<BattleAnimationConstantField>();

    [Header("Stats changers")]
    public List<BattleStatsGetter> statGetters = new List<BattleStatsGetter>();
    [Header("Trigger Conditions for move modifiers")]
    public List<MoveTriggerCondition> triggerConditionsMove = new List<MoveTriggerCondition>();

    public virtual StatusField CreateStatusInstance()
    {
        Flowchart flowchartInstance = flowchart != null ? Instantiate(flowchart) : null;
        StatusField se = new StatusField(this, flowchartInstance, CloneStatGetters());
        se.minTurns = minTurnDuration;
        se.addedRangeTurns = extraTurnRange;

        if (percentageDamagePerRound != 0)
        {
            BattleTriggerOnRoundEndDamage roundEndDamageTrigger = new BattleTriggerOnRoundEndDamage(se);
            se.AddBattleTrigger(roundEndDamageTrigger);

            // Animation
            BattleTrigger animTrigger = new BattleTriggerRoundEndAnimations(triggerAnimations);
            BattleTrigger messageTrigger = new BattleTriggerOnRoundEndDialog(
                        new BattleTriggerMessageData(
                            flowchartInstance,
                            onTriggerFlowchartBlock,
                            new Dictionary<string, string>()
                        )
                    );
            se.AddBattleTrigger(animTrigger);
            se.AddBattleTrigger(messageTrigger);
        }

        // Generic move use logic
        foreach (MoveTriggerCondition moveTriggerCon in triggerConditionsMove)
        {
            BattleTriggerOnPokemonMove moveTrigger = new BattleTriggerOnPokemonMove(
                null, // We pass null because we dont want to check for a specific pokemon
                moveTriggerCon.moveMods.Clone(),
                moveTriggerCon.triggerCondition.Clone() as TriggerConditionMove
            );
            moveTrigger.cancelMoveInstead = moveTriggerCon.cancelMoveInstead;
            se.AddBattleTrigger(moveTrigger);
        }

        se.AddBattleTrigger(new BattleTriggerOnRoundEndStatusDrop(se));

        return se;
    }

    public List<BattleStatsGetter> CloneStatGetters()
    {
        List<BattleStatsGetter> battleStatsGetters = new List<BattleStatsGetter>();
        foreach(BattleStatsGetter statGetter in statGetters)
        {
            BattleStatsGetter stat = new BattleStatsGetter();
            stat.statMultiplier = statGetter.statMultiplier.Copy();
            stat.affectConditions = statGetter.affectConditions;
            battleStatsGetters.Add(stat);
        }
        return battleStatsGetters;
    }

    public void HandleVisualStart(StatusField sf)
    {
        // We add the constant animations to the battle animator
        foreach (BattleAnimationConstantField bac in constantAnimations)
        {
            // Battle animator will then asign them to the StatusField instance when they are created
            // So that they can be destroyed later
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventConstantFieldAnim(sf, bac));
        }
    }
}

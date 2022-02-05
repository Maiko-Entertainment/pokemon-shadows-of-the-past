using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectConfusion : StatusEffect
{
    public string hitSelfBlock = "Confusion Success";

    public StatusEffectConfusion (PokemonBattleData pokemon, Flowchart flowchart): base(pokemon, flowchart)
    {
        effectId = StatusEffectId.Confused;
        minTurns = 2;
        addedRangeTurns = 2;
        captureRateBonus = 10;
        onEndFlowchartBlock = "Confusion Lose";
    }

    public override void Initiate()
    {
        BattleTriggerOnMoveConfusion confusionTrigger = new BattleTriggerOnMoveConfusion(pokemon, this);
        battleTriggers.Add(confusionTrigger);
        foreach (BattleTrigger bt in battleTriggers)
        {
            BattleMaster.GetInstance()?
                .GetCurrentBattle()?.AddTrigger(
                    bt
                );
        }
        base.Initiate();
    }

    public void HandleConfusionSuccess()
    {
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                message,
                hitSelfBlock,
                new Dictionary<string, string>()
                {
                    { "pokemon", pokemon.GetName() }
                }
            )
        ));
    }
}

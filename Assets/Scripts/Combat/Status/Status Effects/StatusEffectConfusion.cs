using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectConfusion : StatusEffect
{
    public string hitSelfBlock = "Confusion Success";

    public StatusEffectConfusion (PokemonBattleData pokemon): base(pokemon)
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
        base.Initiate();
    }

    public void HandleConfusionSuccess()
    {
        Flowchart battleFlow = BattleAnimatorMaster.GetInstance().battleFlowchart;
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                battleFlow,
                hitSelfBlock,
                new Dictionary<string, string>()
                {
                    { "pokemon", pokemon.GetName() }
                }
            )
        ));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectParalyzed : StatusEffect
{
    public float chance = .25f;
    public float speedMod = 0.5f;
    public StatusEffectParalyzed(PokemonBattleData pokemon) : base(pokemon)
    {
        minTurns = 99999;
        effectId = StatusEffectId.Paralyzed;
        captureRateBonus = 20;
        onWarningFlowchartBlock = "Paralyzed Warning";
        isPrimary = true;
    }

    public override void Initiate()
    {
        BattleTriggerOnMoveParalyzedCancel sleepMoveCancelTrigger = new BattleTriggerOnMoveParalyzedCancel(pokemon, this);
        battleTriggers.Add(sleepMoveCancelTrigger);
        base.Initiate();
    }
}

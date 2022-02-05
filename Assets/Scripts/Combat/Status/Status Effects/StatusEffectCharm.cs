using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectCharm : StatusEffect
{
    public StatusEffectCharm(PokemonBattleData pokemon): base(pokemon)
    {
        effectId = StatusEffectId.Charmed;
        minTurns = 99999;
        captureRateBonus = 60;
    }
    public override void Initiate()
    {
        List<BattleAnimation> animations = BattleAnimatorMaster.GetInstance().GetStatusEffectData(effectId).hitAnims;
        BattleTrigger animTrigger = new BattleTriggerRoundEndAnimations(
                       pokemon,
                       pokemon,
                       animations
                   );
        // Needs trigger to reduce physical attack damage
        battleTriggers.Add(animTrigger);
        foreach (BattleTrigger bt in battleTriggers)
        {
            BattleMaster.GetInstance()?
                .GetCurrentBattle()?.AddTrigger(
                    bt
                );
        }
        base.Initiate();
    }
}

using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectCharm : StatusEffect
{
    public BattleTriggerDamageMods mods = new BattleTriggerDamageMods(0.25f);
    public StatusEffectCharm(PokemonBattleData pokemon, StatusEffectData seData) : base(pokemon, seData, null)
    {
        effectId = StatusEffectId.Charmed;
        minTurns = 99999;
        // CaptureRateBonus = 180;
        onEndFlowchartBlock = "Charm Lose";
        gainStatusBlockName = "Charm Gain";
    }
    public override void Initiate()
    {
        BattleTriggerPokemonDamageDeal reduceDamageTrigger = new BattleTriggerPokemonDamageDeal(pokemon, mods);
        BattleTriggerOnDamageRemoveStatus removeOnDamageTrigger = new BattleTriggerOnDamageRemoveStatus(pokemon,this, new List<DamageSummarySource>() { DamageSummarySource.Move });
        // Charmed pokemon deal less damage
        battleTriggers.Add(reduceDamageTrigger);
        battleTriggers.Add(removeOnDamageTrigger);

        List<BattleAnimation> anims = BattleAnimatorMaster.GetInstance().GetStatusEffectData(effectId).hitAnims;
        foreach (BattleAnimation anim in anims)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(
                new BattleAnimatorEventPokemonMoveAnimation(pokemon, pokemon, anim)
            );
        }
        base.Initiate();
    }
}

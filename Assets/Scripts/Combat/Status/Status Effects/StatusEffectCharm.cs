using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectCharm : StatusEffect
{
    public BattleTriggerDamageMods mods = new BattleTriggerDamageMods(0.25f);
    public StatusEffectCharm(PokemonBattleData pokemon): base(pokemon)
    {
        effectId = StatusEffectId.Charmed;
        minTurns = 99999;
        captureRateBonus = 150;
        onEndFlowchartBlock = "Charm Lose";
    }
    public override void Initiate()
    {
        BattleTriggerDamagePokemonChange reduceDamageTrigger = new BattleTriggerDamagePokemonChange(pokemon, mods);
        BattleTriggerOnDamageRemoveStatus removeOnDamageTrigger = new BattleTriggerOnDamageRemoveStatus(pokemon,this, new List<DamageSummarySource>() { DamageSummarySource.Move });
        // Charmed pokemon deal less damage
        battleTriggers.Add(reduceDamageTrigger);
        battleTriggers.Add(removeOnDamageTrigger);
        base.Initiate();
    }
}

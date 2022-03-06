using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectCharm : StatusEffect
{
    public BattleTriggerDamageMods mods = new BattleTriggerDamageMods(0.5f);
    public StatusEffectCharm(PokemonBattleData pokemon): base(pokemon)
    {
        effectId = StatusEffectId.Charmed;
        minTurns = 99999;
        captureRateBonus = 100;
    }
    public override void Initiate()
    {
        BattleTriggerDamagePokemonChange reduceDamageTrigger = new BattleTriggerDamagePokemonChange(pokemon, mods);
        // Charmed pokemon deal less damage
        battleTriggers.Add(reduceDamageTrigger);
        base.Initiate();
    }
}

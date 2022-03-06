using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHopeless : StatusEffect
{
    public BattleTriggerDamageMods mods;
    public StatusEffectHopeless(PokemonBattleData pokemon): base(pokemon)
    {
        minTurns = 999999;
        effectId = StatusEffectId.Hopeless;
        stopEscape = true;
        mods = new BattleTriggerDamageMods(0.5f);
    }

    public override void Initiate()
    {
        BattleTriggerBeforeDamage damageReducer = new BattleTriggerBeforeDamage(pokemon, mods);
        damageReducer.maxTriggers = 999999;
        battleTriggers.Add(damageReducer);
        base.Initiate();
    }
}

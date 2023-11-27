using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerPokemonDamageDeal : BattleTriggerBeforeDamage
{
    public BattleTriggerPokemonDamageDeal(PokemonBattleData damageDealer, BattleTriggerDamageMods mods):
        base(damageDealer, mods)
    {

    }

    public BattleTriggerPokemonDamageDeal(PokemonBattleData damageDealer, BattleTriggerDamageMods mods, TriggerCondition triggerCondition) :
        base(damageDealer, mods, triggerCondition)
    {

    }

    public override bool Execute(BattleEventTakeDamage damageEvent)
    {
        if (maxTriggers > 0 && damageEvent.damageSummary.pokemonSource == pokemon && triggerCondition.MeetsConditions(pokemon))
        {
            damageEvent.damageSummary.ApplyMods(mods);
        }
        else
        {
            maxTriggers++;
        }
        return base.Execute(damageEvent);
    }
}

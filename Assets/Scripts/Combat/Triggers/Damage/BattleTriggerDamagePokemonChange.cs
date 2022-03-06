using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerDamagePokemonChange : BattleTriggerBeforeDamage
{
    public BattleTriggerDamagePokemonChange(PokemonBattleData pokemonToChangeDamageThatDeals, BattleTriggerDamageMods mods):
        base(pokemonToChangeDamageThatDeals, mods)
    {

    }

    public override bool Execute(BattleEventTakeDamage damageEvent)
    {
        if (maxTriggers > 0 && damageEvent.damageSummary.pokemonSource == pokemon)
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

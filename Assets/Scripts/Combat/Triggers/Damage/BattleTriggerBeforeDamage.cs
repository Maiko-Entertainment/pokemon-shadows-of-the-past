using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerBeforeDamage : BattleTriggerOnPokemon
{
    public BattleTriggerDamageMods mods;
    public BattleTriggerBeforeDamage(PokemonBattleData damageTaker, BattleTriggerDamageMods mods) : base(damageTaker, true)
    {
        eventId = BattleEventId.pokemonTakeDamage;
        this.mods = mods;
    }

    public virtual bool Execute(BattleEventTakeDamage damageEvent)
    {
        if (maxTriggers > 0 && damageEvent.pokemon == pokemon)
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

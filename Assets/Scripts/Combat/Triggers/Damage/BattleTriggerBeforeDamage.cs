using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerBeforeDamage : BattleTriggerOnPokemon
{
    public BattleTriggerDamageMods mods;

    protected TriggerCondition triggerCondition = new TriggerCondition();

    public BattleTriggerBeforeDamage(PokemonBattleData damageTaker, BattleTriggerDamageMods mods) : base(damageTaker, true)
    {
        eventId = BattleEventId.pokemonTakeDamage;
        this.mods = mods;
    }

    public BattleTriggerBeforeDamage(PokemonBattleData damageTaker, BattleTriggerDamageMods mods, TriggerCondition triggerCondition) : base(damageTaker, true)
    {
        eventId = BattleEventId.pokemonTakeDamage;
        this.mods = mods;
        this.triggerCondition = triggerCondition;
    }

    public virtual bool Execute(BattleEventTakeDamage damageEvent)
    {
        if (maxTriggers > 0 && damageEvent.pokemon == pokemon && triggerCondition.MeetsConditions(pokemon))
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

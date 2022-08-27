using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnDamageRemoveStatus : BattleTriggerOnPokemonDamage
{
    public StatusEffect se;
    public List<DamageSummarySource> acceptableSources = new List<DamageSummarySource>();
    public BattleTriggerOnDamageRemoveStatus(PokemonBattleData pokemon, StatusEffect se, List<DamageSummarySource> acceptableSources):
        base(pokemon)
    {
        this.acceptableSources = acceptableSources;
        this.se = se;
    }

    public override bool Execute(BattleEventTakeDamageSuccess battleEvent)
    {
        if (battleEvent.pokemon.battleId == pokemon.battleId)
        {
            if (acceptableSources.Contains(battleEvent.damageEvent.damageSummary.damageSource))
            {
                se.HandleOwnRemove();
            }
        }
        return base.Execute(battleEvent);
    }
}

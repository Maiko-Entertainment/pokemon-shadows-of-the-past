using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerDrainOnMoveDamage : BattleTriggerOnPokemonDamage
{
    public MoveData move;
    public float drainMultiplier;
    public BattleTriggerDrainOnMoveDamage(PokemonBattleData dealer, MoveData move, float drainMultiplier = 0.5f): base(dealer, false)
    {
        this.move = move;
        this.drainMultiplier = drainMultiplier;
        maxTriggers = 1;
        turnsLeft = 1;
    }

    public override bool Execute(BattleEventTakeDamageSuccess battleEvent)
    {
        if (maxTriggers > 0)
        {
            DamageSummary ds = battleEvent.damageEvent.damageSummary;
            PokemonBattleData dealer = battleEvent.damageEvent.damageSummary.pokemonSource;
            if (dealer == pokemon && ds.damageSource == DamageSummarySource.Move && ds.sourceId == (int)move.moveId)
            {
                int damage = ds.damageAmount;
                int drainAmount = (int)(drainMultiplier * damage);
                // Negative drain is used for recoil
                if (drainAmount > 0)
                {
                    BattleMaster.GetInstance().GetCurrentBattle()?.AddPokemonHealEvent(dealer, new HealSummary(drainAmount, HealSource.Move, (int)move.moveId));
                }
                else if (drainAmount < 0)
                {
                    BattleMaster.GetInstance().GetCurrentBattle()?.AddDamageDealtEvent(dealer, new DamageSummary(PokemonTypeId.Undefined, drainAmount * -1, DamageSummarySource.MoveEffect, (int)move.moveId, ds.advantageType, pokemon));
                }
            }
            else
            {
                maxTriggers++;
            }
        }
        return base.Execute(battleEvent);
    }
}

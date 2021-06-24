using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventTakeDamage : BattleEventPokemon
{
    public DamageSummary damageSummary;
    public BattleEventTakeDamage(PokemonBattleData pokemon, DamageSummary summary):base(pokemon)
    {
        eventId = BattleEventId.pokemonTakeDamage;
        damageSummary = summary;
    }

    public override void Execute()
    {
        base.Execute();
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        bool wasFaintedBefore = pokemon.IsFainted();
        int resultingHealth = bm.ApplyDamage(this);
        if (resultingHealth <= 0 && !wasFaintedBefore)
        {
            bm.AddPokemonFaintEvent(this);
        }
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventTakeDamage(this, resultingHealth));
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventTypeAdvantage(damageSummary.advantageType));
    }

}

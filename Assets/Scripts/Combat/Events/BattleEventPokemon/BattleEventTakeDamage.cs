using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventTakeDamage : BattleEventPokemon
{
    public DamageSummary damageSummary;
    public int resultingHealth = 0;
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
        resultingHealth = bm.ApplyDamage(this);
        bm.AddEvent(new BattleEventTakeDamageSuccess(this));
        if (resultingHealth <= 0 && !wasFaintedBefore)
        {
            bm.AddPokemonFaintEvent(this);
        }
        switch (damageSummary.advantageType)
        {
            case BattleTypeAdvantageType.normal:
                AudioClip normalHit = BattleAnimatorMaster.GetInstance().normalClip;
                BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPlaySound(normalHit, 1, true));
                break;
            case BattleTypeAdvantageType.superEffective:
                AudioClip superEffectiveClip = BattleAnimatorMaster.GetInstance().superEffectiveClip;
                BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPlaySound(superEffectiveClip,1,true));
                break;
            case BattleTypeAdvantageType.resists:
                AudioClip weakClip = BattleAnimatorMaster.GetInstance().weakClip;
                BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPlaySound(weakClip, 1, true));
                break;
        }
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventTakeDamage(this, resultingHealth));
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventTypeAdvantage(damageSummary.advantageType));
    }

}

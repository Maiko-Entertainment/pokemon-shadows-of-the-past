using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventPokemonHeal : BattleAnimatorEvent
{
    public BattleEventPokemonHeal battleEvent;
    public int targetHealth;

    public BattleAnimatorEventPokemonHeal(BattleEventPokemonHeal battleEvent, int targetHealth)
    {
        this.battleEvent = battleEvent;
        this.targetHealth = targetHealth;
        eventType = BattleAnimatorEventType.PokemonInfoChange;
    }

    public override void Execute()
    {
        float time = BattleAnimatorMaster.GetInstance().UpdateHealthBar(battleEvent.pokemon, targetHealth);
        base.Execute();
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim(time);
    }
    public override string ToString()
    {
        return "Heal - " + battleEvent.summary.ToString(); ;
    }
}

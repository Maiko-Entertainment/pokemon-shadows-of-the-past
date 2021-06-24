using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BattleAnimatorEventTakeDamage: BattleAnimatorEvent
{
    public BattleEventTakeDamage battleEvent;
    public int targetHealth;

    public BattleAnimatorEventTakeDamage(BattleEventTakeDamage battleEvent, int targetHealth)
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
       return "Damage - " + battleEvent.damageSummary.ToString(); ;
    }
}

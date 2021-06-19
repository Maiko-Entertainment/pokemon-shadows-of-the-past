using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventTakeDamage: BattleAnimatorEvent
{
    public BattleEventTakeDamage battleEvent;
    public int targetHealth;

    public BattleAnimatorEventTakeDamage(BattleEventTakeDamage battleEvent, int targetHealth)
    {
        this.battleEvent = battleEvent;
        this.targetHealth = targetHealth;
    }

    public override void Execute()
    {
        float time = BattleAnimatorMaster.GetInstance().UpdateHealthBar(battleEvent.pokemon, targetHealth);
        base.Execute();
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim(time);
    }

}

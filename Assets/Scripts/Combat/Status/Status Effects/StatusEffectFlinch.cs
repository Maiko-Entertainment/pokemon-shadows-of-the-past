using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectFlinch : StatusEffect
{
    public StatusEffectFlinch(PokemonBattleData pokemon): base(pokemon)
    {
        isPrimary = false;
        effectId = StatusEffectId.Flinch;
        minTurns = 1;
        addedRangeTurns = 0;
    }

    public override void Initiate()
    {
        BattleTriggerOnMoveSleepCancel cancelMoveTrigger = new BattleTriggerOnMoveSleepCancel(pokemon, new UseMoveMods(PokemonTypeId.Unmodify));
        battleTriggers.Add(cancelMoveTrigger);
        cancelMoveTrigger.turnsLeft = 1;
        base.Initiate();
    }
}

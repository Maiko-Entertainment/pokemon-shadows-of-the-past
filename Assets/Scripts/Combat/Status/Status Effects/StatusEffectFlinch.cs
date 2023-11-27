using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectFlinch : StatusEffect
{
    public StatusEffectFlinch(PokemonBattleData pokemon, StatusEffectData seData) : base(pokemon, seData, null)
    {
        // IsPrimary = false;
        effectId = StatusEffectId.Flinch;
        minTurns = 1;
        addedRangeTurns = 0;
        gainStatusBlockName = "Flinch Gain";
    }

    public override void Initiate()
    {
        BattleTriggerOnMoveSleepCancel cancelMoveTrigger = new BattleTriggerOnMoveSleepCancel(pokemon, new UseMoveMods(null));
        BattleTriggerOnRoundEndStatusDrop remove = new BattleTriggerOnRoundEndStatusDrop(this);
        battleTriggers.Add(cancelMoveTrigger);
        battleTriggers.Add(remove);
        cancelMoveTrigger.turnsLeft = 1;
        base.Initiate();
    }
}

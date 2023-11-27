using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectProtected : StatusEffect
{
    public StatusEffectProtected(PokemonBattleData pokemon, StatusEffectData seData) : base(pokemon, seData, null)
    {
        effectId = StatusEffectId.Protected;
    }

    public override void Initiate()
    {
        BattleTriggerOnMoveCancel cancel = new BattleTriggerOnMoveCancel(pokemon);
        cancel.blockName = "Protected";
        BattleTriggerOnMoveCancel cancelProtect = new BattleTriggerOnMoveCancel(pokemon);
        cancelProtect.chance = 0.5f;
        cancelProtect.moveTags = new List<MoveTags>() { MoveTags.Protect };
        cancel.blockName = "Move Miss";
        BattleTriggerOnRoundEndStatusDrop remove = new BattleTriggerOnRoundEndStatusDrop(this);
        battleTriggers.Add(cancel);
        battleTriggers.Add(remove);
        base.Initiate();
    }
}

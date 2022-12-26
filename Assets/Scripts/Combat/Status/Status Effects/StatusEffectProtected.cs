using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectProtected : StatusEffect
{
    public StatusEffectProtected(PokemonBattleData pokemon): base(pokemon)
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
        BattleTriggerOnPokemonRoundEndRemoveStatus remove = new BattleTriggerOnPokemonRoundEndRemoveStatus(this);
        battleTriggers.Add(cancel);
        battleTriggers.Add(remove);
        base.Initiate();
    }
}

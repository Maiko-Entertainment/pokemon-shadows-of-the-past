using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonRoundEndRemoveStatus : BattleTriggerOnRoundEnd
{
    public StatusEffect status;

    public BattleTriggerOnPokemonRoundEndRemoveStatus(StatusEffect status): base()
    {
        this.status = status;
        maxTriggers = 1;
    }

    public override bool Execute(BattleEventRoundEnd battleEvent)
    {
        status.HandleOwnRemove();
        return base.Execute(battleEvent);
    }
}

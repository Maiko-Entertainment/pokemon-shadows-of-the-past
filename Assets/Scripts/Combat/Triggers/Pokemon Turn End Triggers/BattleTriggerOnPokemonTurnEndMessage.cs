using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleTriggerOnPokemonTurnEndMessage : BattleTriggerOnPokemonRoundEnd
{
    public BattleTriggerMessageData message;
    public BattleTriggerOnPokemonTurnEndMessage(PokemonBattleData pokemon, BattleTriggerMessageData message):
        base(pokemon)
    {
        this.message = message;
    }

    public override bool Execute(BattleEventRoundEnd battleEvent)
    {
        BattleAnimatorMaster.GetInstance()?.AddEvent(
            new BattleAnimatorEventNarrative(message)
        );
        return base.Execute(battleEvent);
    }

    public override string ToString()
    {
        return base.ToString() + " - Message";
    }
}

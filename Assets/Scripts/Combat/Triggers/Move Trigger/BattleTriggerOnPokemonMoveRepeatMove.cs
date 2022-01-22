using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonMoveRepeatMove : BattleTriggerOnPokemonMove
{
    public StatusEffectMoveCharge status;
    public BattleTriggerOnPokemonMoveRepeatMove(PokemonBattleData pokemon, UseMoveMods mods, StatusEffectMoveCharge status): base(pokemon, mods, true)
    {
        this.status = status;
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {
        if (battleEvent.pokemon == pokemon && maxTriggers > 0)
        {
            if (status.move == battleEvent.move)
            {
                float powerMod = status.MultiplyMultiplier(2);
                useMoveMods.powerMultiplier = powerMod;
                battleEvent.moveMods.Implement(useMoveMods);
            }
            else
            {
                status.HandleOwnRemove();
            }
        }
        else
        {
            maxTriggers++;
        }
        return base.Execute(battleEvent);
    }
}

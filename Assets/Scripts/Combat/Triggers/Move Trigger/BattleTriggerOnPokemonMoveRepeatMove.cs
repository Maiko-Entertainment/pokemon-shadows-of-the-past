using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonMoveRepeatMove : BattleTriggerOnPokemonMove
{
    public MoveData move;
    public float triggerPowerMultiplierAcum = 1f;

    public StatusEffect statusSource;
    public bool endStatusOnMoveFailToRepeat = true;

    public BattleTriggerOnPokemonMoveRepeatMove(PokemonBattleData pokemon, UseMoveMods mods, MoveData move) : base(pokemon, mods, true)
    {
        this.move = move;
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {
        if (battleEvent.pokemon == pokemon && maxTriggers > 0)
        {
            if (move == battleEvent.move)
            {
                useMoveMods.powerMultiplier *= triggerPowerMultiplierAcum;
                battleEvent.moveMods.Implement(useMoveMods);
            }
            else if (statusSource != null && endStatusOnMoveFailToRepeat)
            {
                statusSource.HandleOwnRemove();
            }
        }
        else
        {
            maxTriggers++;
        }
        return base.Execute(battleEvent);
    }
}

using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectMoveCharge : StatusEffect
{
    public MoveData move;
    public float currentMultiplier = 1;

    public StatusEffectMoveCharge(PokemonBattleData pkmn, MoveData move, StatusEffectData seData): base(pkmn, seData, null)
    {
        this.move = move;
        effectId = StatusEffectId.MoveCharge;
        minTurns = 99999;
    }

    public float MultiplyMultiplier(float mult)
    {
        currentMultiplier *= mult;
        return currentMultiplier;
    }

    public override void Initiate()
    {
        BattleTrigger statusTrigger = new BattleTriggerOnPokemonMoveRepeatMove(pokemon, new UseMoveMods(null), move);

        // Needs trigger to reduce physical attack damage
        battleTriggers.Add(statusTrigger);
        foreach (BattleTrigger bt in battleTriggers)
        {
            BattleMaster.GetInstance()?
                .GetCurrentBattle()?.AddTrigger(
                    bt
                );
        }
        base.Initiate();
    }
}

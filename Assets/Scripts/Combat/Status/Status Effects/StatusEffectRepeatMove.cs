using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectRepeatMove : StatusEffect
{
    public MoveData forceMove;
    public StatusEffectRepeatMove(PokemonBattleData pokemon, MoveData forceMove): base(pokemon)
    {
        this.forceMove = forceMove;
        effectId = StatusEffectId.RepeatMove;
        minTurns = 2;
        addedRangeTurns = 0;
    }

    public override void Initiate()
    {
        BattleTriggerOnPokemonChangeMoveUsed chageMoveEvent = new BattleTriggerOnPokemonChangeMoveUsed(
                pokemon,
                new UseMoveMods(PokemonTypeId.Unmodify),
                forceMove
            );
        battleTriggers.Add(chageMoveEvent);
        base.Initiate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonChangeMoveUsed : BattleTriggerOnPokemonMove
{
    public MoveData moveReplacement;
    public BattleTriggerOnPokemonChangeMoveUsed(PokemonBattleData pokemon, UseMoveMods useMoveMods, MoveData moveReplacement) : 
        base(pokemon, useMoveMods, true)
    {
        this.moveReplacement = moveReplacement;
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {
        if (battleEvent.pokemon.battleId == pokemon.battleId && maxTriggers > 0 && useMoveMods != null)
        {
            battleEvent.moveMods.Implement(useMoveMods);
            battleEvent.move = moveReplacement;
        }
        else
        {
            maxTriggers++;
        }
        return true;
    }
}

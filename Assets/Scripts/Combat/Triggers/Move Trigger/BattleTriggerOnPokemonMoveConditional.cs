using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonMoveConditional : BattleTriggerOnPokemonMove
{
    public int maxPower = 200;
    bool showAbility = false;

    public BattleTriggerOnPokemonMoveConditional(PokemonBattleData pokemon, UseMoveMods useMoveMods, bool showAbility) : base(pokemon, useMoveMods, true)
    {
        eventId = BattleEventId.pokemonUseMove;
        this.showAbility = showAbility;
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {
        MoveData move = battleEvent.move;
        if (pokemon.battleId == battleEvent.pokemon.battleId && move.GetPower(pokemon) <= maxPower)
        {
            battleEvent.moveMods.Implement(useMoveMods);
            if (showAbility)
            {
                BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorAbility(pokemon));
            }
        }
        return true;
    }
}

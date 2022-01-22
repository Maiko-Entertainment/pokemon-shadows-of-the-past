using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonBurn : BattleTriggerOnPokemonMove
{
    public BattleTriggerOnPokemonBurn(PokemonBattleData pokemon): base(pokemon, null, true)
    {
        useMoveMods = new UseMoveMods(PokemonTypeId.Unmodify);
        useMoveMods.powerMultiplier = 0.5f; 
    }

    public override bool Execute(BattleEvent battleEvent)
    {
        BattleEventUseMove be = (BattleEventUseMove)battleEvent;
        if (be.move.GetAttackCategory() == MoveCategoryId.physical && be.pokemon == pokemon)
        {
            be.moveMods.Implement(useMoveMods);
        }
        return base.Execute(be);
    }
}

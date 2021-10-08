using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonBurn : BattleTriggerOnPokemon
{
    public BattleTriggerOnPokemonBurn(PokemonBattleData pokemon) : base(pokemon, true)
    {

    }

    public override bool Execute(BattleEvent battleEvent)
    {
        BattleEventUseMove be = (BattleEventUseMove)battleEvent;
        if (be.move.GetAttackCategory() == MoveCategoryId.physical && be.pokemon == pokemon)
        {
            UseMoveMods mods = new UseMoveMods(PokemonTypeId.Unmodify);
            mods.powerMultiplier = 0.5f;
            be.moveMods.Implement(mods);
        }
        return base.Execute(be);
    }
}

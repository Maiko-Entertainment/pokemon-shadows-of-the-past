using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonMoveTargetPokemon : BattleTriggerOnPokemonMove
{
    public UseMoveMods mods;
    public BattleTriggerOnPokemonMoveTargetPokemon(PokemonBattleData pokemon, UseMoveMods mods): base(pokemon, mods, true)
    {
        this.mods = mods;
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {
        PokemonBattleData target = BattleMaster.GetInstance().GetCurrentBattle().GetTarget(battleEvent.pokemon, battleEvent.move.targetType);
        if (maxTriggers > 0 && target == pokemon)
        {
            maxTriggers--;
            battleEvent.moveMods.Implement(mods);
        }
        return true;
    }
}

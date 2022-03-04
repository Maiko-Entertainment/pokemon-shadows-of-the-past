using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectCharged : StatusEffect
{
    public StatusEffectCharged(PokemonBattleData pokemon): base(pokemon)
    {
        effectId = StatusEffectId.Charged;
        minTurns = 1;
        addedRangeTurns = 0;
    }

    public override void Initiate()
    {
        UseMoveMods mods = new UseMoveMods(PokemonTypeId.Unmodify);
        mods.powerMultiplier = 2;
        BattleTriggerOnPokemonMoveDangerTypeMod trigger = new BattleTriggerOnPokemonMoveDangerTypeMod(pokemon, mods, PokemonTypeId.Electric, false);
        trigger.healthTarget = 1;
        base.Initiate();
    }
}

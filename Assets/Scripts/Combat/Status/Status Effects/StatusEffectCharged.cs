using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectCharged : StatusEffect
{
    public StatusEffectCharged(PokemonBattleData pokemon, StatusEffectData seData) : base(pokemon, seData, null)
    {
        effectId = StatusEffectId.Charged;
        minTurns = 1;
        addedRangeTurns = 0;
        gainStatusBlockName = "Charged Gain";
    }

    public override void Initiate()
    {
        UseMoveMods mods = new UseMoveMods(null);
        mods.powerMultiplier = 2;
        List<TypeData> electric = new List<TypeData>(){ TypesMaster.Instance.GetTypeData("Electric") };
        BattleTriggerOnPokemonMoveDangerTypeMod trigger = new BattleTriggerOnPokemonMoveDangerTypeMod(pokemon, mods, electric, false);
        trigger.healthTarget = 1;
        base.Initiate();
    }
}

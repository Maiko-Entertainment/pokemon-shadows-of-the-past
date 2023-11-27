using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectParalyzed : StatusEffect
{
    public float chance = .25f;
    public float speedMod = 0.5f;
    public StatusEffectParalyzed(PokemonBattleData pokemon, StatusEffectData seData) : base(pokemon, seData, null)
    {
        minTurns = 99999;
        effectId = StatusEffectId.Paralyzed;
        // InmuneTypes = new List<TypeData> { TypesMaster.Instance.GetTypeData("Electric") };
        onWarningFlowchartBlock = "Paralyzed Warning";
        gainStatusBlockName = "Paralyze Gain";
        // IsPrimary = true;
    }

    public override void Initiate()
    {
        BattleTriggerOnMoveCancelChance sleepMoveCancelTrigger = new BattleTriggerOnMoveCancelChance(pokemon, chance);
        battleTriggers.Add(sleepMoveCancelTrigger);
        base.Initiate();
    }
}

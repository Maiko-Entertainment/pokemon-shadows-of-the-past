using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/Modify Stat Drop")]
public class AbilityDataPreventStatDrop : AbilityData
{
    public StatsChangeModifyInstructions instrucctions = new StatsChangeModifyInstructions();

    public override void Initialize(PokemonBattleData pokemon)
    {
        BattleTriggerOnPokemonChangeStats trigger = new BattleTriggerOnPokemonChangeStatsModify(pokemon, instrucctions, true);
        BattleMaster.GetInstance().GetCurrentBattle()?.AddTrigger(trigger);
        base.Initialize(pokemon);
    }
}

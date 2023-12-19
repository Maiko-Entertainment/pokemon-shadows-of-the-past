using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/On Field Bonus")]
public class AbilityDataOnStatusFieldBoost : AbilityData
{
    [Header("Stats changers")]
    public List<BattleStatsGetter> statGetters = new List<BattleStatsGetter>();

    public override void Initialize(PokemonBattleData pokemon)
    {
        List<BattleStatsGetter> specificStatGetters = new List<BattleStatsGetter> { };
        foreach (BattleStatsGetter sg in statGetters)
        {
            specificStatGetters.Add(sg.ApplyToPokemon(pokemon));
        }
        foreach (BattleStatsGetter sg in specificStatGetters)
        {
            BattleMaster.GetInstance()?.GetCurrentBattle()?.AddStatGetter(sg);
        }
        BattleTriggerCleanUp cleanUpOnPokemonLeave = new BattleTriggerCleanUp(pokemon);
        cleanUpOnPokemonLeave.onExecute += (BattleEventPokemon po) => RemoveStatGetters(specificStatGetters);
        BattleMaster.GetInstance().GetCurrentBattle().AddTrigger(cleanUpOnPokemonLeave);
        base.Initialize(pokemon);
    }

    public void RemoveStatGetters(List<BattleStatsGetter> getters)
    {
        foreach (BattleStatsGetter sg in getters)
        {
            BattleMaster.GetInstance()?.GetCurrentBattle()?.RemoveStatGetter(sg);
        }
    }
}

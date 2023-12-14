using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Trigger Condition/At least one Status Effect")]
public class TriggerConditionStatusEffect : TriggerConditionData
{
    public List<StatusEffectData> hasAtLeastOneOf = new List<StatusEffectData>();
    public override bool MeetsConditions(PokemonBattleData pokemon)
    {
        List<StatusEffect> list = pokemon.GetStatusEffects();
        foreach (StatusEffect effect in list)
        {
            if (hasAtLeastOneOf.Contains(effect.effectData))
            {
                return !invertCondition;
            }
        }
        return invertCondition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Trigger Condition/Pokemon HP")]
public class TriggerConditionPokemonHPData : TriggerConditionData
{
    [Range(0f, 1f)]
    public float minHealthTreshold = 0f;
    [Range(0f, 1f)]
    public float maxHealthTreshold = 1f;

    public override bool MeetsConditions(PokemonBattleData pokemon)
    {
        float pokemonHPPercentage = pokemon.GetPokemonCurrentHealth() / (float) pokemon.GetMaxHealth();
        bool condition = minHealthTreshold <= pokemonHPPercentage && maxHealthTreshold >= pokemonHPPercentage && base.MeetsConditions(pokemon); ;
        condition = invertCondition ? !condition : condition;
        return condition;
    }
}

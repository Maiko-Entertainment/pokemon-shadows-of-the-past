using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TriggerCondition
{
    // Use public conditions to set basic filters in the unity editor
    [Header("Pokemon Conditions")]
    [Range(0f, 1f)]
    public float minHealthTreshold = 0f;
    [Range(0f, 1f)]
    public float maxHealthTreshold = 1f;

    [Tooltip("Empty to not filter by type")]
    public List<TypeData> pokemonIsAtLeastOneOfTypes = new List<TypeData>();

    // Used for triggers that require a certain move used
    public virtual bool MeetsConditions(PokemonBattleData pokemon, MoveData moveUsed)
    {
        return MeetsConditions(pokemon);
    }

    // Check for Pokemon Conditions
    public virtual bool MeetsConditions(PokemonBattleData pokemon)
    {
        float percentageHealth = (float) pokemon.GetPokemonCurrentHealth() / pokemon.GetMaxHealth(); ;
        bool meetsHealth = percentageHealth >= minHealthTreshold && percentageHealth <= maxHealthTreshold;
        bool meetsTypeRequirements = pokemonIsAtLeastOneOfTypes.Count == 0 || UtilsMaster.ContainsAtLeastOne(pokemonIsAtLeastOneOfTypes, pokemon.GetTypes());
        return meetsHealth && meetsTypeRequirements;
    }

    public virtual TriggerCondition Clone()
    {
        TriggerCondition clone = new TriggerCondition();
        clone.maxHealthTreshold = maxHealthTreshold;
        clone.minHealthTreshold = minHealthTreshold;
        clone.pokemonIsAtLeastOneOfTypes = new List<TypeData>(pokemonIsAtLeastOneOfTypes);
        return clone;
    }
}

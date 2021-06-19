using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PokemonCaughtData
{
    public PokemonBaseData pokemonBase;
    public string pokemonName;
    public int level;
    public int experience;
    public int damageTaken;
    public StatusEffectId statusEffectId;
    public PokemonNatureId natureId;
    public AbilityId abilityId;
    public List<MoveEquipped> moves = new List<MoveEquipped>();

    public int GetLevel()
    {
        return level;
    }
    public bool IsFainted()
    {
        return damageTaken >= GetCurrentStats().health;
    }

    public int ChangeHealth(int change)
    {
        damageTaken = Mathf.Clamp(damageTaken - change, 0, GetCurrentStats().health);
        int maxHealth = GetCurrentStats().health;
        return maxHealth - damageTaken;
    }
    public int GetCurrentHealth()
    {
        int maxHealth = GetCurrentStats().health;
        return Mathf.Clamp(maxHealth - damageTaken, 0, maxHealth);
    }

    public PokemonBaseData GetPokemonBaseData()
    {
        return pokemonBase;
    }

    public List<MoveEquipped> GetAvailableMoves()
    {
        List<MoveEquipped> availableMoves = new List<MoveEquipped>();
        foreach (MoveEquipped move in moves)
        {
            if (move.IsAvailable())
                availableMoves.Add(move);
        }
        return availableMoves;
    }

    public int GetStatValue(int level, int statBase, bool natureBoost)
    {
        float natureValue = natureBoost ? 1.2f : 1f; 
        int value = (int) Mathf.Max(5f + (0.02f * statBase) * level * natureValue, 1);
        return value;
    }

    public int GetHealthValue(int level, int statBase)
    {
        return (int)Mathf.Max(10f + (0.02f * statBase) * level + level, 1);
    }

    public PokemonBaseStats GetCurrentStats()
    {
        PokemonBaseStats pokemonBaseStats = new PokemonBaseStats();
        int level = GetLevel();
        pokemonBaseStats.health = GetHealthValue(
            level, pokemonBase.baseStats.health);
        pokemonBaseStats.attack = GetStatValue(
            level, pokemonBase.baseStats.attack,
            natureId.Equals(PokemonNatureId.ruthless));
        pokemonBaseStats.defense = GetStatValue(
            level, pokemonBase.baseStats.defense,
            natureId.Equals(PokemonNatureId.careful));
        pokemonBaseStats.spAttack = GetStatValue(
            level, pokemonBase.baseStats.spAttack,
            natureId.Equals(PokemonNatureId.cunning));
        pokemonBaseStats.spDefense = GetStatValue(
            level, pokemonBase.baseStats.spDefense,
            natureId.Equals(PokemonNatureId.reserved));
        pokemonBaseStats.speed = GetStatValue(
            level, pokemonBase.baseStats.speed,
            natureId.Equals(PokemonNatureId.restless));
        return pokemonBaseStats;
    }
}

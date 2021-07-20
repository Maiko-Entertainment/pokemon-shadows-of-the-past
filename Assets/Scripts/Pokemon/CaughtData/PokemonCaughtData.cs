using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PokemonCaughtData
{
    public PokemonBaseData pokemonBase;
    public string pokemonName;
    public int level;
    public int experience = 0;
    public int damageTaken = 0;
    public StatusEffectId statusEffectId = StatusEffectId.None;
    public PokemonNatureId natureId;
    public AbilityId abilityId;
    public List<MoveEquipped> moves = new List<MoveEquipped>();

    public PokemonCaughtData() { }
    public AudioClip GetCry() { return pokemonBase.GetCry(); }


    public PokemonCaughtData Copy()
    {
        PokemonCaughtData newInsntace = new PokemonCaughtData();
        newInsntace.pokemonBase = pokemonBase;
        newInsntace.pokemonName = pokemonName;
        newInsntace.level = level;
        newInsntace.experience = experience;
        newInsntace.damageTaken = damageTaken;
        newInsntace.statusEffectId = statusEffectId;
        newInsntace.natureId = natureId;
        newInsntace.abilityId = abilityId;
        List<MoveEquipped> movesInstance = new List<MoveEquipped>();
        foreach(MoveEquipped move in moves)
        {
            movesInstance.Add(move.Copy());
        }
        newInsntace.moves = movesInstance;
        return newInsntace;
    }

    public PokemonCaughtData(PokemonEncounter encounter)
    {
        pokemonBase = encounter.pokemon;
        pokemonName = encounter.pokemon.species;
        level = encounter.baseLevel + Random.Range(0, encounter.extraLevelRange+1);
        natureId = encounter.GetRandomNature();
        abilityId = pokemonBase.GetRandomAbility();
        moves = encounter.GetMovesEquipped();
    }
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
    public List<MoveEquipped> GetMoves()
    {
        return moves;
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

    public List<PokemonTypeId> GetTypes()
    {
        return GetPokemonBaseData().types;
    }
    public int GetExperience()
    {
        return experience;
    }
    public int GetTotalExperienceToNextLevel()
    {
        return GetTotalExperienceToNextLevel(GetLevel());
    }
    public int GetTotalExperienceToNextLevel(int level)
    {
        int amount = (int)(
              1f   * Mathf.Pow(level, 2.5f)
        );
        return amount;
    }

    public int GetRemainingExperienceToNextLevel()
    {
        return GetRemainingExperienceToNextLevel(GetLevel());
    }
    public int GetRemainingExperienceToNextLevel(int level)
    {
        return GetTotalExperienceToNextLevel(level) - experience;
    }

    public LevelUpSummary GainExp(int exp)
    {
        int initialLevel = GetLevel();
        int remainingExp = experience + exp;
        int toNext = GetTotalExperienceToNextLevel();
        List<MoveData> movesLearned = new List<MoveData>();
        while (remainingExp >= toNext)
        {
            remainingExp -= toNext;
            LevelUp();
            movesLearned.AddRange(pokemonBase.GetMovesLearnedForLevel(GetLevel()));
        }
        experience = remainingExp;
        
        return new LevelUpSummary(initialLevel, GetLevel(), movesLearned);
    }

    public void LevelUp()
    {
        experience = 0;
        level += 1;
    }
}

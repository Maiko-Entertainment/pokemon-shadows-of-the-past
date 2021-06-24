using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PokemonBattleData
{
    public PokemonCaughtData pokemon;
    // Modifier levels
    public PokemonBattleStats statsLevel = new PokemonBattleStats();
    public List<StatusEffect> statusEffects = new List<StatusEffect>();
    // Works for pokemon that have moves/items that change typing
    public List<PokemonTypeId> inBattleTypes = new List<PokemonTypeId>();
    public AbilityId abilityId;

    public static int minMaxStatLevelChange = 6;
    
    // Handles abilities, items enter triggers and more right 
    // before the pokemon enters
    public void Initiate()
    {
        inBattleTypes = pokemon.GetPokemonBaseData().types;
        AbilityData ad = AbilityMaster.GetInstance().GetAbility(abilityId);
        ad.Initialize(this);
    }

    public string GetName()
    {
        return pokemon.pokemonName;
    }

    public List<PokemonTypeId> GetTypeIds()
    {
        return inBattleTypes;
    }

    public PokemonCaughtData GetPokemonCaughtData()
    {
        return pokemon;
    }

    public bool IsFainted()
    {
        return GetPokemonCaughtData().IsFainted();
    }

    public int ChangeHealth(int change)
    {
        return pokemon.ChangeHealth(change);
    }

    public float GetMainStatMultiplier(int statChangeLevel)
    {
        if (statChangeLevel > 0)
            return 1f + statChangeLevel * 0.5f;
        else
            return 2f / (2f - statChangeLevel);
    }

    public PokemonBattleStats GetBattleStats()
    {
        PokemonBattleStats finalStats = new PokemonBattleStats();
        PokemonBattleStats statsChangeLevel = GetBattleStatsChangeLevels();
        PokemonBaseStats baseStats = pokemon.GetCurrentStats();
        finalStats.attack = (int)(GetMainStatMultiplier(statsChangeLevel.attack) * baseStats.attack);
        finalStats.defense = (int)(GetMainStatMultiplier(statsChangeLevel.defense) * baseStats.defense);
        finalStats.spAttack = (int)(GetMainStatMultiplier(statsChangeLevel.spAttack) * baseStats.spAttack);
        finalStats.spDefense = (int)(GetMainStatMultiplier(statsChangeLevel.spDefense) * baseStats.spDefense);
        finalStats.speed = (int)(GetMainStatMultiplier(statsChangeLevel.speed) * baseStats.speed);
        return finalStats;
    }

    public int GetPokemonHealth()
    {
        return pokemon.GetCurrentStats().health;
    }

    public int GetPokemonCurrentHealth()
    {
        return pokemon.GetCurrentHealth();
    }

    public void ChangeStatsLevel(PokemonBattleStats statsChange)
    {
        statsLevel.attack = Mathf.Clamp(statsLevel.attack + statsChange.attack, -minMaxStatLevelChange, minMaxStatLevelChange);
        statsLevel.defense = Mathf.Clamp(statsLevel.defense + statsChange.defense, -minMaxStatLevelChange, minMaxStatLevelChange);
        statsLevel.spAttack = Mathf.Clamp(statsLevel.spAttack + statsChange.spAttack, -minMaxStatLevelChange, minMaxStatLevelChange);
        statsLevel.spDefense = Mathf.Clamp(statsLevel.spDefense + statsChange.spDefense, -minMaxStatLevelChange, minMaxStatLevelChange);
        statsLevel.speed = Mathf.Clamp(statsLevel.speed + statsChange.speed, -minMaxStatLevelChange, minMaxStatLevelChange);
        statsLevel.accuracy = Mathf.Clamp(statsLevel.accuracy + statsChange.accuracy, -minMaxStatLevelChange, minMaxStatLevelChange);
        statsLevel.evasion = Mathf.Clamp(statsLevel.evasion + statsChange.evasion, -minMaxStatLevelChange, minMaxStatLevelChange);
        statsLevel.critical = Mathf.Clamp(statsLevel.critical + statsChange.critical, -minMaxStatLevelChange, minMaxStatLevelChange);
    }

    public PokemonBattleStats GetBattleStatsChangeLevels()
    {
        return statsLevel;
    }

    public void AddStatusEffect(StatusEffect status)
    {
        statusEffects.Add(status);
        status.Initiate();
    }

    public bool AlreadyHasPrimaryStatus()
    {
        foreach (StatusEffect status in statusEffects)
            if (status.isPrimary)
                return true;
        return false;
    }

    public StatusEffect GetCurrentPrimaryStatus()
    {
        foreach (StatusEffect status in statusEffects)
            if (status.isPrimary)
                return status;
        return null;
    }

    public List<MoveEquipped> GetMoves()
    {
        return pokemon.moves;
    }

}

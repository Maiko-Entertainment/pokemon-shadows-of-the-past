using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PokemonBattleData
{
    [HideInInspector] public int battleId = 0;
    public PokemonCaughtData pokemon;
    // Modifier levels
    public PokemonBattleStats statsLevel = new PokemonBattleStats();
    public List<StatusEffect> statusEffects = new List<StatusEffect>();
    // Works for pokemon that have moves/items that change typing
    public List<PokemonTypeId> inBattleTypes = new List<PokemonTypeId>();
    public AbilityData ability;
    public PokemonBattleDataItem heldItem;
    public int roundsInCombat = 0;
    public bool hideLevel = false;

    public static int minMaxStatLevelChange = 6;

    public PokemonBattleData(PokemonCaughtData pokemon, int battleId)
    {
        this.battleId = battleId;
        this.pokemon = pokemon;
        heldItem = new PokemonBattleDataItem(pokemon.GetEquippedItem());
        statusEffects = new List<StatusEffect>();
        inBattleTypes = new List<PokemonTypeId>();
    }

    public PokemonBattleData Copy()
    {
        PokemonBattleData newInstance = new PokemonBattleData(pokemon.Copy(), battleId);
        if (statusEffects == null)
            statusEffects = new List<StatusEffect>();
        foreach (StatusEffect se in statusEffects)
        {
            newInstance.statusEffects.Add(se.Copy(newInstance));
        }
        newInstance.statsLevel = statsLevel.Copy();
        newInstance.ability = ability;
        newInstance.heldItem = heldItem;
        newInstance.roundsInCombat = roundsInCombat;
        newInstance.hideLevel = hideLevel;
        return newInstance;
    }

    // Handles abilities, items enter triggers and more right 
    // before the pokemon enters
    public void Initiate()
    {
        inBattleTypes = pokemon.GetPokemonBaseData().types;
        ability = pokemon.ability;
        heldItem.Initiate(this);
        ability.Initialize(this);
        if (statusEffects == null)
            statusEffects = new List<StatusEffect>();
        foreach (StatusEffect se in statusEffects)
            se.Initiate();
    }

    public string GetName()
    {
        return pokemon.GetName();
    }

    public AudioClip GetCry()
    {
        return pokemon.GetCry();
    }

    public float GetCaptureRate()
    {
        return pokemon.GetPokemonBaseData().captureRate;
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

    public void UnequipItem()
    {
        heldItem.Remove();
        heldItem = null;
    }

    public void EquippedItem(ItemDataOnPokemon item)
    {
        heldItem = new PokemonBattleDataItem(item);
        heldItem.Initiate(this);
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
        finalStats.accuracy = statsLevel.accuracy;
        finalStats.evasion = statsLevel.evasion;
        finalStats.critical = statsLevel.critical;
        return finalStats;
    }
    public int GetMaxHealth()
    {
        return pokemon.GetCurrentStats().health;
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
    public void ResetStatLevels()
    {
        statsLevel = new PokemonBattleStats();
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

    public void RemoveStatusEffect(StatusEffectId statusId)
    {
        List<StatusEffect> statusNew = new List<StatusEffect>();
        foreach(StatusEffect se in statusEffects)
        {
            if (se.effectId != statusId)
            {
                statusNew.Add(se);
            }
            else
            {
                se.Remove();
            }
        }
        statusEffects = statusNew;
    }

    public void RemoveAllStatusEffects(bool onlyMinors = false)
    {
        foreach (StatusEffect se in statusEffects)
        {
            if (!onlyMinors || (onlyMinors && !se.isPrimary))
                se.Remove();
        }
        statusEffects = new List<StatusEffect>();
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
        if (statusEffects != null)
        {
            foreach (StatusEffect status in statusEffects)
                if (status.isPrimary)
                    return status;
            return null;
        }
        return null;
    }
    public List<StatusEffect> GetNonPrimaryStatus()
    {
        List<StatusEffect> statuses = new List<StatusEffect>();
        if (statusEffects != null)
        {
            foreach (StatusEffect status in statusEffects)
                if (!status.isPrimary)
                    statuses.Add(status);
            return statuses;
        }
        return statuses;
    }

    public List<MoveEquipped> GetMoves()
    {
        return pokemon.moves;
    }

    public void ReduceMovePP(MoveData move)
    {
        List<MoveEquipped> moves = GetMoves();
        foreach(MoveEquipped m in moves)
        {
            if (m.move.GetId() == move.GetId())
                m.ChangeTimesUsed(1);
        }
    }

}

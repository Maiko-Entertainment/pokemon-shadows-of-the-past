﻿using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PokemonCaughtData
{
    public string pokemonName;
    public PokemonBaseData pokemonBase;
    public int level;
    public int experience = 0;
    public int damageTaken = 0;
    public StatusEffectId statusEffectId = StatusEffectId.None;
    public PokemonNatureId natureId;
    public AbilityId abilityId;
    public bool isMale = true;
    public float friendship = 0;
    public ItemDataOnPokemon equippedItem;
    public List<MoveEquipped> moves = new List<MoveEquipped>();
    public List<MoveEquipped> learnedMoves = new List<MoveEquipped>();
    public bool isShadow = false;

    public static int MAX_FRIENDSHIP = 255;

    public PokemonCaughtData() { }
    public AudioClip GetCry() { return pokemonBase.GetCry(); }
    public Sprite GetIcon() { return pokemonBase.GetIcon(); }

    public PokemonCaughtData(PersistedPokemon pkmn)
    {
        pokemonBase = PokemonMaster.GetInstance().GetPokemonData(pkmn.pokemonId);
        pokemonName = pkmn.pokemonName;
        level = pkmn.level;
        experience = pkmn.experience;
        damageTaken = pkmn.damageTaken;
        statusEffectId = pkmn.statusEffectId;
        natureId = pkmn.natureId;
        abilityId = pkmn.abilityId;
        isShadow = pkmn.isShadow;
        friendship = pkmn.friendship;
        isMale = pkmn.isMale;
        friendship = pkmn.friendship;
        equippedItem = (ItemDataOnPokemon) ItemMaster.GetInstance().GetItem(pkmn.equipedItem);
        foreach(PersistedPokemonMove me in pkmn.moves)
        {
            moves.Add(new MoveEquipped(me));
        }
        foreach (PersistedPokemonMove me in pkmn.learnedMoves)
        {
            learnedMoves.Add(new MoveEquipped(me));
        }
        CheckForLearnedMoves(pkmn.level);
    }

    public PersistedPokemon GetSave()
    {
        PersistedPokemon pp = new PersistedPokemon();
        pp.pokemonId = pokemonBase.pokemonId;
        pp.pokemonName = pokemonName;
        pp.level = level;
        pp.experience = experience;
        pp.damageTaken = damageTaken;
        pp.statusEffectId = statusEffectId;
        pp.natureId = natureId;
        pp.abilityId = abilityId;
        pp.isShadow = isShadow;
        pp.friendship = friendship;
        pp.isMale = isMale;
        pp.equipedItem = equippedItem ? equippedItem.GetItemId() : ItemId.None;
        foreach (MoveEquipped me in moves)
        {
            pp.moves.Add(me.GetSave());
        }
        foreach (MoveEquipped me in learnedMoves)
        {
            pp.learnedMoves.Add(me.GetSave());
        }
        return pp;
    }
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
        newInsntace.isShadow = isShadow;
        newInsntace.isMale = isMale;
        newInsntace.equippedItem = equippedItem;
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
        level = encounter.baseLevel + Random.Range(0, encounter.extraLevelRange+1);
        natureId = encounter.GetRandomNature();
        abilityId = pokemonBase.GetRandomAbility();
        moves = encounter.GetMovesEquipped();
        friendship = encounter.pokemon.baseFriendship;
        float random = Random.value;
        isMale = random < encounter.pokemon.maleChance;
    }

    public string GetName()
    {
        return pokemonName != "" && pokemonName != null ? pokemonName : pokemonBase.species;
    }

    public void SetName(string newName)
    {
        pokemonName = newName;
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
        float shadowBoost = isShadow ? 1.2f : 1f;
        float natureValue = natureBoost ? 1 * level : 0;
        int value = (int) Mathf.Max((5f + (0.02f * statBase) * level) * shadowBoost + Mathf.Ceil(natureValue * 0.5f), 1);
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
    public float GetFriendship()
    {
        return isShadow ? 0 : friendship;
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
              level * 5 + Mathf.Pow(level * 15, 2.5f) / 2000
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
    public ItemDataOnPokemon GetEquippedItem()
    {
        return equippedItem;
    }
    public void UnequipItem()
    {
        equippedItem = null;
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
            toNext = GetTotalExperienceToNextLevel();
            movesLearned.AddRange(pokemonBase.GetMovesLearnedForLevel(GetLevel()));
        }
        experience = remainingExp;
        CheckForLearnedMoves(GetLevel());
        return new LevelUpSummary(initialLevel, GetLevel(), movesLearned);
    }

    public float GainFriendship(int friendshipGained)
    {
        friendship = Mathf.Min(friendship + friendshipGained, MAX_FRIENDSHIP);
        return friendship;
    }

    public void LevelUp()
    {
        experience = 0;
        level += 1;
    }

    public bool TryToLearnMove(MoveData moveData)
    {
        bool isMoveInLearnedMoves = false;
        foreach (MoveEquipped me in learnedMoves)
        {
            try
            {
                if (moveData.moveId == me.move.moveId)
                {
                    isMoveInLearnedMoves = true;
                    break;
                }
            }
            catch
            {
                Debug.LogError(me.move);
                Debug.LogError(moveData);
            }
        }
        if (!isMoveInLearnedMoves)
        {
            MoveEquipped me = new MoveEquipped(moveData);
            foreach (MoveEquipped move in moves)
            {
                if (moveData.moveId == move.move.moveId)
                {
                    isMoveInLearnedMoves = true;
                    break;
                }
            }
            if (moves.Count < 4 && !isMoveInLearnedMoves)
            {
                moves.Add(me);
            }
            learnedMoves.Add(me);
            return true;
        }
        return false;
    }

    // returns a list of NEW moves the pokemon learns at certain level
    public List<PokemonMoveLearn> CheckForLearnedMoves()
    {
        return CheckForLearnedMoves(GetLevel());
    }
    public List<PokemonMoveLearn> CheckForLearnedMoves(int level)
    {
        List<PokemonMoveLearn> newMovesLearned = new List<PokemonMoveLearn>();
        List<PokemonMoveLearn> levelUpMoves = pokemonBase.GetLearntMovesByLevel(level);

        // Add available TM Moves
        foreach(MoveData tm in pokemonBase.GetTMMoves())
        {
            if (InventoryMaster.GetInstance().HasTMForMove(tm.moveId))
                TryToLearnMove(tm);
        }
        // Add level up moves
        foreach (PokemonMoveLearn availableMove in levelUpMoves)
        {
            bool result = TryToLearnMove(availableMove.move);
            if (result)
            {
                newMovesLearned.Add(availableMove);
            }
        }
        return newMovesLearned;
    }

    public List<MoveEquipped> GetLearnedMoves()
    {
        return learnedMoves;
    }

    public void EquipItem(ItemDataOnPokemon item)
    {
        if (equippedItem != null)
        {
            InventoryMaster.GetInstance().ChangeItemAmount(equippedItem.GetItemId(), 1);
        }
        InventoryMaster.GetInstance().ChangeItemAmount(item.GetItemId(), -1);
        equippedItem = item;
    }

    public void EquipMove(MoveEquipped move, MoveEquipped swapFor)
    {
        // Index of swapFor element if it already exists
        int index = -1;
        // Index of move element to swap for new
        int indexOfNew = -1;
        foreach (MoveEquipped m in moves)
        {
            if (m.move.moveId == swapFor.move.moveId)
            {
                index = moves.IndexOf(m);
            }
            else if (m.move.moveId == move.move.moveId)
            {
                indexOfNew = moves.IndexOf(m);
            }
        }
        // Swaps the new move for the swaped move if the new move is already equipped
        if (index >= 0)
        {
            moves[index] = move;
        }
        moves[indexOfNew] = swapFor;
    }
}

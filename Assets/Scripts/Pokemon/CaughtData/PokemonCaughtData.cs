using System.Collections.Generic;
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

    public static int MAX_FRIENDSHIP = 255;

    public PokemonCaughtData() { }
    public AudioClip GetCry() { return pokemonBase.GetCry(); }

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
        friendship = encounter.pokemon.baseFriendship;
    }

    public string GetName()
    {
        return pokemonName != "" ? pokemonName : pokemonBase.species;
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
        float natureValue = natureBoost ? 1 * level : 0; 
        int value = (int) Mathf.Max(5f + (0.02f * statBase) * level + natureValue, 1);
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
        return friendship;
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

    // returns a list of NEW moves the pokemon learns at certain level
    public List<PokemonMoveLearn> CheckForLearnedMoves(int level)
    {
        List<PokemonMoveLearn> newMovesLearned = new List<PokemonMoveLearn>();
        List<MoveEquipped> newCompleteMovesLearned = new List<MoveEquipped>();
        List<PokemonMoveLearn> allMovesLearned = pokemonBase.GetLearntMovesByLevel(level);
        newCompleteMovesLearned.AddRange(learnedMoves);

        foreach (PokemonMoveLearn availableMove in allMovesLearned)
        {
            bool isMoveInLearnedMoves = false;
            foreach(MoveEquipped me in learnedMoves)
            {
                if (availableMove.move.moveId == me.move.moveId)
                {
                    isMoveInLearnedMoves = true;
                    break;
                }
            }
            if (!isMoveInLearnedMoves)
            {
                newMovesLearned.Add(availableMove);
                MoveEquipped me = new MoveEquipped(availableMove.move);
                newCompleteMovesLearned.Add(me);
                foreach (MoveEquipped move in moves)
                {
                    if (availableMove.move.moveId == move.move.moveId)
                    {
                        isMoveInLearnedMoves = true;
                        break;
                    }
                }
                if (moves.Count < 4 && !isMoveInLearnedMoves)
                {
                    moves.Add(me);
                }
            }
        }
        learnedMoves = newCompleteMovesLearned;
        return newMovesLearned;
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
}

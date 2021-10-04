﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Pokemon/BaseData")]
public class PokemonBaseData : ScriptableObject
{
    public PokemonBaseId pokemonId;
    public string species;
    public Sprite icon;
    public AudioClip cry;
    public PokemonBaseStats baseStats;
    public List<PokemonTypeId> types;
    public List<PokemonBaseAbility> abilities;
    public float maleChance = 0.5f;
    public bool isGenderless = false;
    public float captureRate;
    public float baseFriendship;
    public float baseExp = 50;
    public List<PokemonMoveLearn> levelUpMoves;
    public List<PokemonBaseEvolution> evolutions;
    public PokemonAnimationController battleAnimation;
    public string pokedexEntry;

    public AudioClip GetCry() { return cry; }

    public List<MoveData> GetMovesLearnedForLevel(int level)
    {
        List<MoveData> moves = new List<MoveData>();
        foreach(PokemonMoveLearn move in levelUpMoves)
        {
            if (move.learnType == PokemonMoveLearnType.Level &&
                move.levelRequired == level)
            {
                moves.Add(move.move);
            }
        }
        return moves;
    }

    public AbilityId GetRandomAbility()
    {
        int total = 0;
        foreach (PokemonBaseAbility naturePriority in abilities)
        {
            total += naturePriority.abilityPriority;
        }
        int neededPrioritySum = Random.Range(0, total);
        total = 0;
        foreach (PokemonBaseAbility naturePriority in abilities)
        {
            total += naturePriority.abilityPriority;
            if (total >= neededPrioritySum)
                return naturePriority.abilityId;
        }
        return AbilityId.Intimidate;
    }

    public string GetPokedexEntry()
    {
        return pokedexEntry;
    }

    public List<PokemonMoveLearn> GetLearntMovesByLevel(int level)
    {
        List<PokemonMoveLearn> levelUpMoves = new List<PokemonMoveLearn>();
        foreach(PokemonMoveLearn pml in this.levelUpMoves)
        {
            if (pml.learnType == PokemonMoveLearnType.Level && pml.levelRequired <= level)
                levelUpMoves.Add(pml);
        }
        return levelUpMoves;
    }
}

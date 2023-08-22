using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Pokemon/BaseData")]
public class PokemonBaseData : ScriptableObject
{
    public PokemonBaseId pokemonId;
    public string species;
    [SerializeField] protected Sprite icon;
    protected AudioClip cry;
    public PokemonBaseStats baseStats;
    public List<PokemonTypeId> types;
    public List<PokemonBaseAbility> abilities;
    public float maleChance = 0.5f;
    public bool isGenderless = false;
    public float captureRate;
    public float baseFriendship;
    public float baseExp = 50;
    public string pokedexEntry;
    public List<PokemonMoveLearn> levelUpMoves;
    public List<MoveData> tmMoves;
    public List<PokemonBaseEvolution> evolutions;
    public PokemonAnimationController battleAnimation;
    public WorldInteractableBrainFollower overWorldPrefab;
    public List<PokemonBaseDataAreas> encounteredIn = new List<PokemonBaseDataAreas>();

    public AudioClip GetCry() {
        string path = ResourceMaster.Instance.GetBasePokemonCryPath(this);
        AudioClip audioCry = Resources.Load<AudioClip>(path);
        return audioCry;
    }
    public Sprite GetIcon()
    {
        string path = ResourceMaster.Instance.GetBaseIconPath(this);
        Sprite audioCry = Resources.Load<Sprite>(path);
        return audioCry;
    }

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

    public List<MoveData> GetTMMoves()
    {
        return tmMoves;
    }

    public PokemonAnimationController GetAnimatorController()
    {
        return battleAnimation;
    }

    public WorldInteractableBrainFollower GetOverWorldPrefab()
    {
        return overWorldPrefab;
    }
}

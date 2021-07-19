using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Encounters/Pokemon Wild Encounter")]
public class PokemonEncounter: ScriptableObject
{
    public PokemonBaseData pokemon;
    public int priority = 4;
    public int baseLevel = 5;
    public int extraLevelRange = 3;
    public List<EncounterNaturePriority> natures = new List<EncounterNaturePriority>();
    public List<MoveData> moves = new List<MoveData>();

    public PokemonCaughtData GetPokemonCaught()
    {
        PokemonCaughtData pkmn = new PokemonCaughtData(this);
        return pkmn;
    }

    public PokemonNatureId GetRandomNature()
    {
        int total = 0;
        foreach(EncounterNaturePriority naturePriority in natures)
        {
            total += naturePriority.priority;
        }
        int neededPrioritySum = Random.Range(0, total);
        total = 0;
        foreach (EncounterNaturePriority naturePriority in natures)
        {
            total += naturePriority.priority;
            if (total >= neededPrioritySum)
                return naturePriority.natureId;
        }
        return PokemonNatureId.careful;
    }

    public List<MoveEquipped> GetMovesEquipped()
    {
        List<MoveEquipped> moves = new List<MoveEquipped>();
        foreach(MoveData move in this.moves)
        {
            moves.Add(new MoveEquipped(move));
        }
        return moves;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Evolution/Min Level")]
public class PokemonEvolutionDataRequirementLevel : PokemonEvolutionDataRequirement
{
    public int minLevel = 36;
    public override bool MeetsRequirement(PokemonCaughtData pokemon)
    {
        return minLevel <= pokemon.GetLevel();
    }
    public override string GetRequirementText()
    {
        return "Lvl " + minLevel;
    }
}

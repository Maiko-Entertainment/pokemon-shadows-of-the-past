using System;
using System.Collections.Generic;

[System.Serializable]
public class PokemonBaseEvolution
{
    public PokemonBaseData pokemon;
    public List<PokemonEvolutionDataRequirement> evolutionRequirements = new List<PokemonEvolutionDataRequirement>();

    public bool CanEvolve(PokemonCaughtData pokemon)
    {
        foreach(PokemonEvolutionDataRequirement er in evolutionRequirements)
        {
            if (!er.MeetsRequirement(pokemon)) return false;
        }
        return true;
    }

    public bool CanEvolve(PokemonCaughtData pokemon, ItemData itemData)
    {
        bool hasItemRequirement = false;
        // First we need to check if at least one of the requirements is item specific
        foreach (PokemonEvolutionDataRequirement er in evolutionRequirements)
        {
            if (er.GetType().IsAssignableFrom(typeof(PokemonEvolutionDataRequirementItemUse)))
            {
                hasItemRequirement = true;
                break;
            }
        }
        if (!hasItemRequirement)
        {
            return false;
        }
        else
        {
            foreach (PokemonEvolutionDataRequirement er in evolutionRequirements)
            {
                if (!er.MeetsRequirement(pokemon, itemData)) return false;
            }
        }
        return true;
    }
}

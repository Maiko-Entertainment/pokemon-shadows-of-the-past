using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonEvolutionDataRequirement : ScriptableObject
{
    public virtual bool MeetsRequirement(PokemonCaughtData pokemon)
    {
        return false;
    }

    // Used for knowing if the pokemon can evolve from an item
    public virtual bool MeetsRequirement(PokemonCaughtData pokemon, ItemData item)
    {
        return true;
    }

    // Use this when the pokemon has evolve, this can be removing the item from inventory
    // Or in the case of shedinja adding it to the party.
    public virtual void ProcessEvolution()
    {

    }

    public virtual string GetRequirementText()
    {
        return string.Empty;
    }
}

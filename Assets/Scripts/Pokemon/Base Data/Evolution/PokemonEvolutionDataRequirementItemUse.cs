using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Evolution/Item Use")]
public class PokemonEvolutionDataRequirementItemUse : PokemonEvolutionDataRequirement
{
    public ItemDataOnPokemonEvolve item;
    // This pokemon can only evolve from using an item
    public override bool MeetsRequirement(PokemonCaughtData pokemon)
    {
        return false;
    }

    public override bool MeetsRequirement(PokemonCaughtData pokemon, ItemData item)
    {
        bool isItemCorrect = this.item.GetItemId() == item.GetItemId();
        return isItemCorrect;
    }

    public override string GetRequirementText()
    {
        return "Use "+ item.GetName();
    }
}

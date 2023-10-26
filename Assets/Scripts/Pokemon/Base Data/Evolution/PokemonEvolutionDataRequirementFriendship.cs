using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Evolution/Min Friendship")]
public class PokemonEvolutionDataRequirementFriendship : PokemonEvolutionDataRequirement
{
    public int minFriendship = 180;
    public override bool MeetsRequirement(PokemonCaughtData pokemon)
    {
        return minFriendship <= pokemon.GetFriendship();
    }
    public override string GetRequirementText()
    {
        return "Friendship " + minFriendship;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Items/Friendship")]
public class ItemDataOnPokemonFriendship : ItemDataOnPokemon
{
    public int friendshipChange = 5;

    public override CanUseResult CanUseOnPokemon(PokemonCaughtData pokemon)
    {
        return new CanUseResult(!pokemon.isShadow, pokemon.isShadow ? "It refuses." : "");
    }

    public override CanUseResult CanUseOnPokemonBattle(PokemonBattleData pokemon)
    {
        return new CanUseResult(false, "It wouldn't have any effect");
    }

    public override void UseOnPokemon(PokemonCaughtData pokemon)
    {
        pokemon.GainFriendship(friendshipChange);
        base.UseOnPokemon(pokemon);
    }
}

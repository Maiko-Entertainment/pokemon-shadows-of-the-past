using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Items/Evolve")]
public class ItemDataOnPokemonEvolve : ItemDataOnPokemon
{
    public override CanUseResult CanUseOnPokemon(PokemonCaughtData pokemon)
    {
        List<PokemonBaseEvolution> evos = pokemon.GetPokemonBaseData().evolutions;
        foreach(PokemonBaseEvolution evo in evos)
        {
            if (evo.evolutionData.evolutionType == PokemonEvolutionType.item && (ItemId)evo.evolutionData.value == itemId)
            {
                return new CanUseResult(true, "The " + GetName() + " is reacting!");
            }
        }
        return new CanUseResult(false, "The "+GetName()+" is not reacting...");
    }

    public override CanUseResult CanUseOnPokemonBattle(PokemonBattleData pokemon)
    {
        return new CanUseResult(false, "You can't use that here");
    }

    public override void UseOnPokemon(PokemonCaughtData pokemon)
    {
        List<PokemonBaseEvolution> evos = pokemon.GetPokemonBaseData().evolutions;
        foreach (PokemonBaseEvolution evo in evos)
        {
            if (evo.evolutionData.evolutionType == PokemonEvolutionType.item && (ItemId)evo.evolutionData.value == itemId)
            {
                UIPauseMenuMaster.GetInstance().CloseAllMenus();
                PokemonBaseData evolution = PokemonMaster.GetInstance().GetPokemonData(evo.pokemonId);
                PokemonMaster.GetInstance().EvolvePokemon(pokemon, evolution);
            }
        }
        base.UseOnPokemon(pokemon);
    }
}

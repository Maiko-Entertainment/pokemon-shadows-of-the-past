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
            if (evo.CanEvolve(pokemon, this))
            {
                return new CanUseResult(true, GetName() + " is reacting!");
            }
        }
        return new CanUseResult(false, GetName()+" is not reacting...");
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
            if (CanUseOnPokemon(pokemon).canUse && evo.CanEvolve(pokemon, this))
            {
                UIPauseMenuMaster.GetInstance().CloseAllMenus();
                PokemonBaseData evolution = evo.pokemon;
                PokemonMaster.GetInstance().EvolvePokemon(pokemon, evolution);
            }
        }
        base.UseOnPokemon(pokemon);
    }
}

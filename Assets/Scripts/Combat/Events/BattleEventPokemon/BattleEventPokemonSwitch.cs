using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonSwitch : BattleEventPokemon
{
    public PokemonBattleData newPokemon;
    public BattleEventPokemonSwitch(PokemonBattleData pokemon, PokemonBattleData newPokemon):
        base(pokemon)
    {
        eventId = BattleEventId.pokemonSwitch;
        this.newPokemon = newPokemon;
    }

    public override void Execute()
    {
        pokemon.RemoveAllStatusEffects(true);
        BattleMaster.GetInstance()?.GetCurrentBattle().SetTeamActivePokemon(newPokemon);
        if (!pokemon.IsFainted())
        {
            BattleAnimatorMaster.GetInstance()?.AddEventBattleFlowcartPokemonText(
                "Switch",
                pokemon
            );
        }
        base.Execute();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventEnterPokemon : BattleAnimatorEventPokemon
{
    public int pokemonHealth;
    public StatusEffect pokemonStatus;
    public BattleAnimatorEventEnterPokemon(PokemonBattleData pokemon) : base(pokemon)
    {
        pokemonHealth = pokemon.GetPokemonCurrentHealth();
        pokemonStatus = pokemon.GetCurrentPrimaryStatus();
    }

    public override void Execute()
    {
        BattleTeamId id = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(pokemon);
        BattleAnimatorMaster.GetInstance().SetTeamPokemon(pokemon, id);
        BattleAnimatorMaster.GetInstance().LoadPokemonsInfo(pokemon, pokemonHealth, pokemonStatus);
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim(0.25f);
        base.Execute();
    }
}

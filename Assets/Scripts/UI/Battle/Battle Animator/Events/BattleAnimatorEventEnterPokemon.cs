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
    }

    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance().LoadPokemonsInfo(pokemon, pokemonHealth);
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim(0f);
        base.Execute();
    }
}

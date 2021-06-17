using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonChangeStat: BattleEventPokemon
{
    public PokemonBattleStats statsLevelChange;
    public BattleEventPokemonChangeStat(PokemonBattleData pokemon, PokemonBattleStats stats):
        base(pokemon)
    {
        eventId = BattleEventId.pokemonChangeStats;
        statsLevelChange = stats;
    }

    public override void Execute()
    {
        base.Execute();
        pokemon.ChangeStatsLevel(statsLevelChange);
    }
}

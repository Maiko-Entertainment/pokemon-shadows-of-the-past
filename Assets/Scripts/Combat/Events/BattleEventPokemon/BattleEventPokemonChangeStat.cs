using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonChangeStat : BattleEventPokemon
{
    public PokemonBattleStats statsLevelChange;
    public BattleEventPokemonChangeStat(PokemonBattleData pokemon, PokemonBattleStats stats) :
        base(pokemon)
    {
        eventId = BattleEventId.pokemonChangeStats;
        statsLevelChange = stats;
    }

    public override void Execute()
    {
        base.Execute();
        pokemon.ChangeStatsLevel(statsLevelChange);
        PokemonBattleStats battleStats = statsLevelChange;
        HandleStatUpDown("Attack", battleStats.attack, pokemon);
        HandleStatUpDown("Defense", battleStats.defense, pokemon);
        HandleStatUpDown("Special Attack", battleStats.spAttack, pokemon);
        HandleStatUpDown("Special Defense", battleStats.spDefense, pokemon);
        HandleStatUpDown("Speed", battleStats.speed, pokemon);
        HandleStatUpDown("Evasion", battleStats.evasion, pokemon);
        HandleStatUpDown("Accuracy", battleStats.accuracy, pokemon);
        HandleStatUpDown("Critical Rate", battleStats.critical, pokemon);
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPokemonUpdateInfobar(pokemon.Copy()));
    }

    public void HandleStatUpDown(string statName, int change, PokemonBattleData pokemon)
    {
        if (change > 0)
        {
            BattleAnimatorMaster.GetInstance().AddStatusChangeEvent(pokemon, change);
            BattleAnimatorMaster.GetInstance().AddEventBattleFlowcartPokemonText(
                "Stat Up", pokemon,
                new Dictionary<string, string>() { { "stat", statName } }
            );
        }
        else if (change < 0)
        {
            BattleAnimatorMaster.GetInstance().AddStatusChangeEvent(pokemon, change);
            BattleAnimatorMaster.GetInstance().AddEventBattleFlowcartPokemonText(
                "Stat Down", pokemon,
                new Dictionary<string, string>() { { "stat", statName } }
            );
        }
    }
}

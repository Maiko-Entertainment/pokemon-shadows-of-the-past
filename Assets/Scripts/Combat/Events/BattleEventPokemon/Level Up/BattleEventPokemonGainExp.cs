using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonGainExp : BattleEventPokemon
{
    public int experience;
    public BattleEventPokemonGainExp(PokemonBattleData pokemon, int experience):
        base(pokemon)
    {
        this.experience = experience;
        eventId = BattleEventId.pokemonGainExp;
    }

    public override void Execute()
    {
        LevelUpSummary summary = pokemon.GetPokemonCaughtData().GainExp(experience);
        PokemonBattleData activePokemon = BattleMaster.GetInstance()?.GetCurrentBattle()?.GetTeamActivePokemon(BattleTeamId.Team1);
        bool isActivePokemon = activePokemon == pokemon;
        if (isActivePokemon)
        {
            BattleAnimatorMaster.GetInstance()?.AddEventBattleFlowcartGainExpText(experience);
            for (int level=summary.initialLevel+1; level <= summary.finalLevel; level++)
            {
                BattleAnimatorMaster.GetInstance()?.AddEvent(
                    new BattleAnimatorEventPokemonLevelUp(
                        pokemon,
                        new LevelUpSummary(level - 1, level, new List<MoveData>())
                    )
                );
                BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPokemonUpdateLevel(pokemon, level));
                BattleAnimatorMaster.GetInstance()?.AddEventBattleFlowcartGainLevelText(pokemon.GetName(), level);
            }
            BattleAnimatorMaster.GetInstance()?.AddEvent(
                new BattleAnimatorEventPokemonLevelUp(pokemon, new LevelUpSummary(summary.finalLevel, summary.finalLevel, new List<MoveData>()))
            );
            BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPokemonUpdateInfobar(pokemon));
        }
        else{
            for (int level = summary.initialLevel + 1; level <= summary.finalLevel; level++)
            {
                BattleAnimatorMaster.GetInstance()?.AddEvent(
                    new BattleAnimatorEventPokemonLevelUp(
                        pokemon,
                        new LevelUpSummary(level - 1, level, new List<MoveData>())
                    )
                );
                BattleAnimatorMaster.GetInstance()?.AddEventBattleFlowcartGainLevelText(pokemon.GetName(), level);
            }
        }
        base.Execute();
    }
}

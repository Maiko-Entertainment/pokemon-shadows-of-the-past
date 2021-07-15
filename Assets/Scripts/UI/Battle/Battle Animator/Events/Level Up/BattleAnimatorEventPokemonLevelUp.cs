using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventPokemonLevelUp : BattleAnimatorEventPokemon
{
    public LevelUpSummary summary;
    public BattleAnimatorEventPokemonLevelUp(PokemonBattleData pokemon, LevelUpSummary summary):
        base(pokemon)
    {
        this.summary = summary;
        eventType = BattleAnimatorEventType.PokemonInfoChange;
}

    public override void Execute()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        float time = 0;
        
        if (bm.GetTeamActivePokemon(BattleTeamId.Team1) == pokemon)
        {
            // Checks if pokemon wont level up
            if (summary.initialLevel == summary.finalLevel)
            {
                time = BattleAnimatorMaster.GetInstance().UpdateExpBar(
                    pokemon,
                    pokemon.pokemon.GetExperience(),
                    pokemon.pokemon.GetTotalExperienceToNextLevel(summary.initialLevel)
                );
            }
            else
            {
                time = BattleAnimatorMaster.GetInstance().UpdateExpBar(
                    pokemon,
                    pokemon.pokemon.GetRemainingExperienceToNextLevel(summary.finalLevel),
                    pokemon.pokemon.GetTotalExperienceToNextLevel(summary.initialLevel)
                );
            }
            
        }
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim(time);
        base.Execute();
    }
    public override string ToString()
    {
        return base.ToString() + " - Update Exp - " + summary.finalLevel;
    }
}

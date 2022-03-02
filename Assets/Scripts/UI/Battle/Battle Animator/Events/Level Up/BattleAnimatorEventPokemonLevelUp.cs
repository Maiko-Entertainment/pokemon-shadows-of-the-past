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
        
        if (bm.GetTeamActivePokemon(BattleTeamId.Team1).battleId == pokemon.battleId)
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
                    pokemon.pokemon.GetTotalExperienceToNextLevel(summary.initialLevel),
                    pokemon.pokemon.GetTotalExperienceToNextLevel(summary.initialLevel)
                );
            }
        }
        AudioClip gainExpClip = BattleAnimatorMaster.GetInstance().expGainClip;
        AudioMaster.GetInstance()?.PlaySfxWithDuration(gainExpClip, time);
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim(time);
        base.Execute();
    }
    public override string ToString()
    {
        return base.ToString() + " - Update Exp - " + summary.finalLevel;
    }
}

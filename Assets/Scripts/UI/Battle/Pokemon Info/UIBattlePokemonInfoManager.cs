using System.Collections.Generic;
using UnityEngine;

public class UIBattlePokemonInfoManager : MonoBehaviour
{
    public UIBattleHealthbar team1Health;
    public UIBattleHealthbar team2Health;

    public void HideTeamInfo(BattleTeamId team, bool instant = false)
    {
        if (team == BattleTeamId.Team1)
        {
            team1Health.FadeOut(instant);
        }
        else
        {
            team2Health.FadeOut(instant);
        }
    }

    public void UpdatePokemonInfo(PokemonBattleData pokemon, bool showIfHidden = false)
    {
        BattleManager battleState = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTeamId team = battleState.GetTeamId(pokemon);
        if (team == BattleTeamId.Team1)
        {
            team1Health.Load(pokemon);
            if (showIfHidden) team1Health.FadeIn();
        }
        else
        {
            team2Health.Load(pokemon);
            if (showIfHidden) team2Health.FadeIn();
        }
    }

    public void UpdatePokemonData(PokemonBattleData pokemon, int health, StatusEffectData status, List<StatusEffectData> minor)
    {
        BattleManager battleState = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTeamId team = battleState.GetTeamId(pokemon);
        if (team == BattleTeamId.Team1)
        {
            team1Health.Load(pokemon);
            // team1Health.FadeIn();
            team1Health.UpdateTarget(health);
            team1Health.UpdateHealth(health);
        }
        else
        {
            team2Health.Load(pokemon);
            // team2Health.FadeIn();
            team2Health.UpdateTarget(health);
            team2Health.UpdateHealth(health);
        }
        UpdateStatus(pokemon, status, minor);
    }

    public float UpdateHealthbar(PokemonBattleData pokemon, int target)
    {
        BattleManager battleState = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTeamId team = battleState.GetTeamId(pokemon);
        if (team == BattleTeamId.Team1)
        {
            return team1Health.UpdateTarget(target);
        }
        else
        {
            return team2Health.UpdateTarget(target);
        }
    }

    public float UpdateExpbar(PokemonBattleData pokemon, int target, int max)
    {
        BattleManager battleState = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTeamId team = battleState.GetTeamId(pokemon);
        if (team == BattleTeamId.Team1)
        {
            return team1Health.UpdateTargetExp(target, max);
        }
        else
        {
            return team2Health.UpdateTargetExp(target, max);
        }
    }
    public void UpdatePokemonLevel(PokemonBattleData pokemon, int level)
    {
        BattleManager battleState = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTeamId team = battleState.GetTeamId(pokemon);
        if (team == BattleTeamId.Team1)
        {
            team1Health.UpdateLevel(level);
        }
        else
        {
            team2Health.UpdateLevel(level);
        }
    }
    public float ShowPokemonAbility(PokemonBattleData pokemon)
    {
        BattleManager battleState = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTeamId team = battleState.GetTeamId(pokemon);
        if (team == BattleTeamId.Team1)
        {
            return team1Health.ShowAbility();
        }
        else
        {
            return team2Health.ShowAbility();
        }
    }

    public void UpdateStatus(PokemonBattleData pokemon, StatusEffectData status, List<StatusEffectData> minors)
    {
        BattleManager battleState = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTeamId team = battleState.GetTeamId(pokemon);
        if (team == BattleTeamId.Team1)
        {
            team1Health.UpdateStatus(status, minors);
        }
        else
        {
            team2Health.UpdateStatus(status, minors);
        }
    }
}

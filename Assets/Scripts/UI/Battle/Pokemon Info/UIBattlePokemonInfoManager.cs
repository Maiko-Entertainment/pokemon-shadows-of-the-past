using System.Collections.Generic;
using UnityEngine;

public class UIBattlePokemonInfoManager : MonoBehaviour
{
    public UIBattleHealthbar team1Health;
    public UIBattleHealthbar team2Health;

    public void HideTeamInfo(BattleTeamId team)
    {
        if (team == BattleTeamId.Team1)
        {
            team1Health.FadeOut();
        }
        else
        {
            team2Health.FadeOut();
        }
    }

    public void UpdateInfo(BattleManager battleState)
    {
        PokemonBattleData team1Pokemon = battleState.GetTeamActivePokemon(BattleTeamId.Team1);
        PokemonBattleData team2Pokemon = battleState.GetTeamActivePokemon(BattleTeamId.Team2);
        UpdatePokemonInfo(team1Pokemon);
        team2Health.Load(team2Pokemon);
    }

    public void UpdatePokemonInfo(PokemonBattleData pokemon)
    {
        BattleManager battleState = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTeamId team = battleState.GetTeamId(pokemon);
        if (team == BattleTeamId.Team1)
        {
            team1Health.Load(pokemon);
        }
        else
        {
            team2Health.Load(pokemon);
        }
    }

    public void UpdatePokemonData(PokemonBattleData pokemon, int health, StatusEffectData status, List<StatusEffectData> minor)
    {
        BattleManager battleState = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTeamId team = battleState.GetTeamId(pokemon);
        if (team == BattleTeamId.Team1)
        {
            team1Health.Load(pokemon);
            team1Health.FadeIn();
            team1Health.UpdateTarget(health);
            team1Health.UpdateHealth(health);
        }
        else
        {
            team2Health.Load(pokemon);
            team2Health.FadeIn();
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

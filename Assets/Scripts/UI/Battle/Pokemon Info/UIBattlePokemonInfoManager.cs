using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattlePokemonInfoManager : MonoBehaviour
{
    public UIBattleHealthbar team1Health;
    public UIBattleHealthbar team2Health;

    public void UpdateInfo(BattleManager battleState)
    {
        PokemonBattleData team1Pokemon = battleState.GetTeamActivePokemon(BattleTeamId.Team1);
        PokemonBattleData team2Pokemon = battleState.GetTeamActivePokemon(BattleTeamId.Team2);
        team1Health.Load(team1Pokemon);
        team2Health.Load(team2Pokemon);
    }

    public void UpdatePokemonData(PokemonBattleData pokemon, int health, StatusEffectData status)
    {
        BattleManager battleState = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTeamId team = battleState.GetTeamId(pokemon);
        if (team == BattleTeamId.Team1)
        {
            team1Health.Load(pokemon);
            team1Health.UpdateTarget(health);
            team1Health.UpdateHealth(health);
        }
        else
        {
            team2Health.Load(pokemon);
            team2Health.UpdateTarget(health);
            team2Health.UpdateHealth(health);
        }
        UpdateStatus(pokemon, status);
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

    public void UpdateStatus(PokemonBattleData pokemon, StatusEffectData status)
    {
        BattleManager battleState = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTeamId team = battleState.GetTeamId(pokemon);
        if (team == BattleTeamId.Team1)
        {
            team1Health.UpdateStatus(status);
        }
        else
        {
            team2Health.UpdateStatus(status);
        }
    }
}

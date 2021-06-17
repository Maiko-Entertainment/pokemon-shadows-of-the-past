using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorMaster : MonoBehaviour
{
    public static BattleAnimatorMaster Instance;

    public Transform background;
    public Transform pokemonTeam1Position;
    public Transform pokemonTeam2Position;

    public BattleManager currentBattle;

    public void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public static BattleAnimatorMaster GetInstance() { return Instance; }

    public void LoadBattle(BattleManager battleState)
    {
        PokemonBattleData team1Pokemon = battleState.GetTeamActivePokemon(BattleTeamId.Team1);
        PokemonBattleData team2Pokemon = battleState.GetTeamActivePokemon(BattleTeamId.Team2);
        SetTeamPokemon(team1Pokemon, BattleTeamId.Team1);
        SetTeamPokemon(team2Pokemon, BattleTeamId.Team2);
    }

    public void SetTeamPokemon(PokemonBattleData pokemon, BattleTeamId teamId)
    {
        if (teamId == BattleTeamId.Team1)
        {
            foreach(Transform p in pokemonTeam1Position)
            {
                Destroy(p.gameObject);
            }
            
        }
        else if (teamId == BattleTeamId.Team2)
        {
            foreach (Transform p in pokemonTeam2Position)
            {
                Destroy(p.gameObject);
            }
        }
        InstantiatePokemonAnim(pokemon, teamId);
    }

    public PokemonAnimationController InstantiatePokemonAnim(PokemonBattleData pokemon, BattleTeamId teamId)
    {
        PokemonAnimationController pkmnInstance = Instantiate(
            pokemon.GetPokemonCaughtData().GetPokemonBaseData()
            .battleAnimation.gameObject, teamId == BattleTeamId.Team1 ? pokemonTeam1Position : pokemonTeam2Position)
            .GetComponent<PokemonAnimationController>();
        if (teamId == BattleTeamId.Team1)
        {
            pkmnInstance.TriggerBack();
        }
        return pkmnInstance;
    }
}

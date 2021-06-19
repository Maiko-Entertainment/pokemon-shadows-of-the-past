using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class BattleAnimatorMaster : MonoBehaviour
{
    public static BattleAnimatorMaster Instance;

    public Transform background;
    public Transform pokemonTeam1Position;
    public Transform pokemonTeam2Position;
    public UIBattleOptionsManager battleOptionsManager;
    public UIBattlePokemonInfoManager battleInfoManager;
    public Flowchart battleFlowchart;

    public BattleAnimatorManager animatorManager = new BattleAnimatorManager();
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
        battleOptionsManager?.LoadMoves();
        battleInfoManager?.UpdateInfo(battleState);
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

    public void GoToNextBattleAnim(float seconds=0)
    {
        StartCoroutine(TriggerNextEvent(seconds));
    }
    IEnumerator TriggerNextEvent(float seconds=0)
    {
        yield return new WaitForSeconds(seconds);
        animatorManager.TriggerNextEvent();
    }

    public void AddEvent(BattleAnimatorEvent newEvent)
    {
        animatorManager.AddEvent(newEvent);
    }


    // Must only be used by Anim events when they are executed
    public void ExecuteMoveFlowchart(BattleEventUseMove moveEvent)
    {
        battleFlowchart.StopAllBlocks();
        battleFlowchart.SetStringVariable("pokemon", moveEvent.pokemon.GetName());
        battleFlowchart.SetStringVariable("move", moveEvent.move.moveName);
        battleFlowchart.ExecuteBlock("Move Use");
    }

    public void ExecuteEffectivenessFlowchart(BattleTypeAdvantageType advantageType)
    {
        switch (advantageType)
        {
            case BattleTypeAdvantageType.superEffective:
                battleFlowchart.ExecuteBlock("Super Effective");
                break;
            case BattleTypeAdvantageType.resists:
                battleFlowchart.ExecuteBlock("Resistant");
                break;
            case BattleTypeAdvantageType.inmune:
                battleFlowchart.ExecuteBlock("Inmune");
                break;
            default:
                GoToNextBattleAnim();
                break;
        }
    }

    public float UpdateHealthBar(PokemonBattleData pokemon, int target)
    {
        return battleInfoManager.UpdateHealthbar(pokemon, target);
    }

    public void ShowTurnOptions()
    {
        battleOptionsManager.Show();
    }
    public void HideTurnOptions()
    {
        battleOptionsManager.Hide();
    }
}

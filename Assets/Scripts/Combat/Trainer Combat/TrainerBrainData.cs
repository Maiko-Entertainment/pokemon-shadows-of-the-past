using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Trainer Brains/Base Brain")]
public class TrainerBrainData : ScriptableObject
{
    public Flowchart flowchart;
    public List<BrainMessageCondition> easyDialogs = new List<BrainMessageCondition>();
    public List<BrainTacticCondition> easyTactics = new List<BrainTacticCondition>();

    protected Flowchart flowchartInstance;
    public virtual void Initialize(BattleManager currentBattle)
    {
        if (flowchart)
        {
            flowchartInstance = Instantiate(flowchart.gameObject).GetComponent<Flowchart>();
            foreach(BrainMessageCondition dialog in easyDialogs)
            {
                dialog.Initiate(currentBattle, flowchartInstance);
            }
        }
        foreach(BrainTacticCondition tactic in easyTactics)
        {
            tactic.Initialice();
        }
    }

    public virtual BattleTurnDesition GetTurnDesition(BattleManager currentBattle)
    {
        BattleTeamData teamData = currentBattle.GetTeamData(BattleTeamId.Team2);
        PokemonBattleData team2Pokemon = teamData.GetActivePokemon();
        BattleTurnDesition turnDesition = new BattleTurnDesition(BattleTeamId.Team2);
        if (team2Pokemon != null)
        {
            List<MoveEquipped> moves = team2Pokemon.GetPokemonCaughtData().GetAvailableMoves();
            if (moves.Count > 0)
            {
                MoveEquipped highiestPriorityMove = null;
                int highestPriority = -99999;
                /*
                int randomIndex = Random.Range(0, moves.Count);
                MoveEquipped move = moves[randomIndex];
                */
                foreach(MoveEquipped move in moves)
                {
                    int priority = move.move.GetPriority(currentBattle, BattleTeamId.Team2);
                    if (priority > highestPriority)
                    {
                        highiestPriorityMove = move;
                        highestPriority = priority;
                    }
                }
                turnDesition =  new BattleTurnDesitionPokemonMove(highiestPriorityMove, team2Pokemon, BattleTeamId.Team2);
            }
            else
            {
                turnDesition = new BattleTurnDesitionPokemonMove(new MoveEquipped(MovesMaster.Instance.GetMove(MoveId.Struggle)), team2Pokemon, BattleTeamId.Team2);
            }
        }
        turnDesition.tactic = GetTacticToUse(currentBattle);
        return turnDesition;
    }

    public virtual TacticData GetTacticToUse(BattleManager currentBattle)
    {
        foreach(BrainTacticCondition tactic in easyTactics)
        {
            if (tactic.CanUse(currentBattle))
            {
                return tactic.tactic;
            }
        }
        return null;
    }

    public virtual PokemonBattleData GetNextPokemon(BattleManager currentBattle)
    {
        BattleTeamData teamData = currentBattle.GetTeamData(BattleTeamId.Team2);
        PokemonBattleData team2Pokemon = teamData.GetFirstAvailabelPokemon();
        return team2Pokemon;
    }
}

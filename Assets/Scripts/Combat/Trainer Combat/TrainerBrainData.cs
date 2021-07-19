using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Trainer Brains/Base Brain")]
public class TrainerBrainData : ScriptableObject
{
    public Flowchart flowchart;

    protected Flowchart flowchartInstance;
    public virtual void Initialize(BattleManager currentBattle)
    {
        flowchartInstance = Instantiate(flowchart.gameObject).GetComponent<Flowchart>();
    }

    public virtual BattleTurnDesition GetTurnDesition(BattleManager currentBattle)
    {
        BattleTeamData teamData = currentBattle.GetTeamData(BattleTeamId.Team2);
        PokemonBattleData team2Pokemon = teamData.GetActivePokemon();
        if (team2Pokemon != null)
        {
            List<MoveEquipped> moves = team2Pokemon.GetPokemonCaughtData().GetAvailableMoves();
            if (moves.Count > 0)
            {
                int randomIndex = Random.Range(0, moves.Count);
                MoveEquipped move = moves[randomIndex];

                return new BattleTurnDesitionPokemonMove(move, team2Pokemon, BattleTeamId.Team2);
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

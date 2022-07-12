using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BrainTacticCondition
{
    public TacticData tactic;
    public BrainMessageConditionType type;
    public float value;
    public int maxUses = 1;

    public bool CanUse(BattleManager bm)
    {
        if (maxUses > 0)
        {
            switch (type)
            {
                case BrainMessageConditionType.trainerPokemonsLeft:
                    int myPokemonLeft = bm.GetTeamData(BattleTeamId.Team2).GetAvailablePokemon().Count;
                    if (myPokemonLeft == (int)value)
                    {
                        maxUses--;
                        return true;
                    }
                    break;
                case BrainMessageConditionType.playersPokemonsLeft:
                    int playersPokemonLeft = bm.GetTeamData(BattleTeamId.Team1).GetAvailablePokemon().Count;
                    if (playersPokemonLeft == (int)value)
                    {
                        maxUses--;
                        return true;
                    }
                    break;
                case BrainMessageConditionType.roundNumber:
                    int turnsPassed = bm.turnsPassed;
                    if (turnsPassed == (int)value)
                    {
                        maxUses--;
                        return true;
                    }
                    break;
            }
        }
        return false;
    }
}

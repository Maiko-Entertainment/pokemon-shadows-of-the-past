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
    protected int timesUsed = 0;

    public bool CanUse(BattleManager bm)
    {
        if (maxUses > timesUsed)
        {
            switch (type)
            {
                case BrainMessageConditionType.trainerPokemonsLeft:
                    int myPokemonLeft = bm.GetTeamData(BattleTeamId.Team2).GetAvailablePokemon().Count;
                    if (myPokemonLeft == (int)value)
                    {
                        timesUsed++;
                        return true;
                    }
                    break;
                case BrainMessageConditionType.playersPokemonsLeft:
                    int playersPokemonLeft = bm.GetTeamData(BattleTeamId.Team1).GetAvailablePokemon().Count;
                    if (playersPokemonLeft == (int)value)
                    {
                        timesUsed++;
                        return true;
                    }
                    break;
                case BrainMessageConditionType.roundNumber:
                    int turnsPassed = bm.turnsPassed;
                    if (turnsPassed == (int)value - 1)
                    {
                        timesUsed++;
                        return true;
                    }
                    break;
            }
        }
        return false;
    }

    public void Initialice()
    {
        timesUsed = 0;
    }
}

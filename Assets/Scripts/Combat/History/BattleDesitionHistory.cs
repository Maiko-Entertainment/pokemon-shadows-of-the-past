using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDesitionHistory
{
    public int turn = 0;
    public BattleTurnDesition desition;

    public BattleDesitionHistory(BattleTurnDesition desition, int turn)
    {
        this.desition = desition;
        this.turn = turn;
    }
}

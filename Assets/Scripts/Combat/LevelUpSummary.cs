using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSummary
{
    public int initialLevel = 0;
    public int finalLevel = 0;
    public List<MoveData> movesLearned = new List<MoveData>();

    public LevelUpSummary(int initialLevel, int finalLevel, List<MoveData> movesLearned)
    {
        this.initialLevel = initialLevel;
        this.finalLevel = finalLevel;
        this.movesLearned = movesLearned;
    }
}

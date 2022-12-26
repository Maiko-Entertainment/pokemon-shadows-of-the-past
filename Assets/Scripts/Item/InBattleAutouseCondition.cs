using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InBattleAutouseCondition
{
    None = 0,
    HalfHealth = 1,
    OnAtLeastOneCurableStatus = 2,
    EndOfRound = 3,
}

using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerBrainDataAdvanced : TrainerBrainData
{
    public override void Initialize(BattleManager currentBattle)
    {
        base.Initialize(currentBattle);
    }

    public override BattleTurnDesition GetTurnDesition(BattleManager currentBattle)
    {
        // TO DO: Create better desition handling later
        return base.GetTurnDesition(currentBattle);
    }
}

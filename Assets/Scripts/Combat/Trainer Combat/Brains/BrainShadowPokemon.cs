using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Trainer Brains/Shadow Brain")]
public class BrainShadowPokemon : TrainerBrainData
{
    public int firsTurnTacticIndex = 0;
    public override BattleTurnDesition GetTurnDesition(BattleManager currentBattle)
    {
        int turns = currentBattle.turnsPassed;
        BattleTurnDesition desition = base.GetTurnDesition(currentBattle);
        if (turns == 0 && firsTurnTacticIndex >= 0 && firsTurnTacticIndex < tacticsAvailable.Count)
        {
            desition.tactic = tacticsAvailable[firsTurnTacticIndex];
        }
        return desition;
    }
}

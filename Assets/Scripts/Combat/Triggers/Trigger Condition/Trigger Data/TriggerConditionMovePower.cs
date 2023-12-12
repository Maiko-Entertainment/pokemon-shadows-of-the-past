using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Trigger Condition/Move Power")]
public class TriggerConditionMovePower : TriggerConditionData
{
    public int minPowerValue = 0;
    public int maxPowerValue = int.MaxValue;

    public override bool MeetsConditions(PokemonBattleData pokemon, MoveData move)
    {
        int movePower = move.GetPower(pokemon);
        bool condition = minPowerValue <= movePower && maxPowerValue >= movePower && base.MeetsConditions(pokemon, move);
        condition = invertCondition ? !condition : condition;
        return condition;
    }
}

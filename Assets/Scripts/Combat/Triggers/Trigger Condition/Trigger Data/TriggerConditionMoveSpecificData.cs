using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Trigger Condition/Move Specific")]
public class TriggerConditionMoveSpecificData : TriggerConditionData
{
    public List<MoveData> isOneOfTheseMoves = new List<MoveData>();

    public override bool MeetsConditions(PokemonBattleData pokemon, MoveData move)
    {
        bool condition = isOneOfTheseMoves.Contains(move) && base.MeetsConditions(pokemon, move);
        condition = invertCondition ? !condition : condition;
        return condition;
    }
}

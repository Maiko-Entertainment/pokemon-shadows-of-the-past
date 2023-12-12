using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Trigger Condition/Move Type")]
public class TriggerConditionMoveType : TriggerConditionData
{
    public List<TypeData> isAtLeastOneOfTypes = new List<TypeData>();

    public override bool MeetsConditions(PokemonBattleData pokemon, MoveData move)
    {
        TypeData moveType = move.GetMoveType();
        bool condition = isAtLeastOneOfTypes.Contains(moveType) && base.MeetsConditions(pokemon, move);
        condition = invertCondition ? !condition : condition;
        return condition;
    }
}

using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Trigger Condition/Move Category")]
public class TriggerConditionMoveCategory : TriggerConditionData
{
    public List<MoveCategoryId> isAtLeastOneOfCateogories = new List<MoveCategoryId>();

    public override bool MeetsConditions(PokemonBattleData pokemon, MoveData move)
    {
        MoveCategoryId moveCategory = move.GetAttackCategory();
        bool condition = isAtLeastOneOfCateogories.Contains(moveCategory) && base.MeetsConditions(pokemon, move);
        condition = invertCondition ? !condition : condition;
        return condition;
    }
}

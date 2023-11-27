using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TriggerConditionMove : TriggerCondition
{
    // Move public conditions for unity editor
    [Header("Move Conditions")]
    [Tooltip("Empty to not filter by type")]
    public List<TypeData> moveIsAtLeastOneOfTypes = new List<TypeData>();
    [Tooltip("None to not filter by category")]
    public List<MoveCategoryId> moveCategories = new List<MoveCategoryId>();
    public int minMovePowerTreshold = 0;
    public int maxMovePowerTrashold = int.MaxValue;

    public bool focusOnEnemiesTargetingPokemonInstead = false;
    // TODO: Add later move tags ones they are ScriptableObjects

    protected List<MoveData> isOneOfTheseMoves = new List<MoveData>();

    public override bool MeetsConditions(PokemonBattleData pokemon, MoveData moveUsed)
    {
        bool meetsTypeRequirements = moveIsAtLeastOneOfTypes.Count == 0 || moveIsAtLeastOneOfTypes.Contains(moveUsed.GetMoveType());
        bool meetsMoveCategory = moveCategories.Count == 0 || moveCategories.Contains(moveUsed.GetAttackCategory());
        bool isBetweenPowerTreshold = moveUsed.GetPower(pokemon) >= minMovePowerTreshold && moveUsed.GetPower(pokemon) <= maxMovePowerTrashold;
        bool meetsSpecificMove = isOneOfTheseMoves.Count == 0 || isOneOfTheseMoves.Contains(moveUsed);
        bool meetsMoveConditions = meetsTypeRequirements && meetsMoveCategory && isBetweenPowerTreshold && meetsSpecificMove;
        return meetsMoveConditions && base.MeetsConditions(pokemon,moveUsed) && MeetsConditions(pokemon);
    }

    public virtual void SetSpecificMovesRequirements(List<MoveData> moves)
    {
        isOneOfTheseMoves = new List<MoveData>(moves);
    }

    public override TriggerCondition Clone()
    {
        return MemberwiseClone() as TriggerCondition;
    }
}

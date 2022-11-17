using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/TM")]
public class ItemDataTM : ItemData
{
    public MoveData moveLearned;

    public override void Use()
    {
        base.Use();
    }

    public override string GetDescription()
    {
        return "Allows certain pokemon to pick this move from their move list.";
    }
}

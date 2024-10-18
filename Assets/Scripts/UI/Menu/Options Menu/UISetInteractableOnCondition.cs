using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetInteractableOnCondition : MonoBehaviour
{
    public List<SpawnConditionDataSaveValue> conditions = new List<SpawnConditionDataSaveValue>();
    public List<SpawnConditionItemInInventory> inventoryConditions = new List<SpawnConditionItemInInventory>();
    public Selectable selectable;

    private void Awake()
    {
        CheckForSpawn();
    }

    protected bool MeetsConditions()
    {
        foreach (SpawnConditionDataSaveValue con in conditions)
        {
            if (!con.IsTrue())
                return false;
        }
        foreach (SpawnConditionItemInInventory con in inventoryConditions)
        {
            if (!con.IsTrue())
                return false;
        }
        return true;
    }

    public void CheckForSpawn()
    {
        selectable.interactable = MeetsConditions();
    }
}

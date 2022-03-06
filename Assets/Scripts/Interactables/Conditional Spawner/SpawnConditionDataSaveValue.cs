using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SpawnConditionDataSaveValue
{
    public SaveElementId saveVariable;
    public SpawnConditionDataValueCheck condition;
    public float value;

    public bool IsTrue()
    {
        SaveElement elem = SaveMaster.Instance.GetSaveElement(saveVariable);
        SaveElementNumber elemNumber = (SaveElementNumber)elem;
        float saveValue = (float)elemNumber.GetValue();
        switch (condition)
        {
            case SpawnConditionDataValueCheck.IsLessThan:
                return saveValue <= value;
            case SpawnConditionDataValueCheck.IsMoreThan:
                return saveValue >= value;
            case SpawnConditionDataValueCheck.IsDifferent:
                return saveValue != value;
            default:
                return saveValue == value;
        }
    }
}

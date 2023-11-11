using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SpawnConditionDataSaveValue
{
    public SaveElementId saveVariable;
    public string saveElementId;
    public SaveElementNumber saveElement;
    public SpawnConditionDataValueCheck condition;
    public float value;

    public string GetId()
    {
        if (string.IsNullOrEmpty(saveElementId))
        {
            if (saveElement != null)
            {
                // TODO: This is legacy and should be removed later
                return saveVariable.ToString();
            }
            return saveElement.GetId();
        }
        return saveElementId;
    }

    public bool IsTrue()
    {
        float saveValue = SaveMaster.Instance.GetSaveElementFloat(GetId());
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

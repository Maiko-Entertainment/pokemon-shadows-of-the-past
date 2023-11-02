using UnityEngine;

[System.Serializable]
public class SpawnConditionDataSaveValue
{
    public string saveVariable;
    public SpawnConditionDataValueCheck condition;
    public float value;

    public bool IsTrue()
    {
        float saveValue = SaveMaster.Instance.GetElementAsFloat(saveVariable);
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

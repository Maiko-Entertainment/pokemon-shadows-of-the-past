using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Quest/Base Quest")]
[System.Serializable]
public class UIQuestData : ScriptableObject
{
    public string title;

    public List<SpawnConditionDataSaveValue> appearCondition = new List<SpawnConditionDataSaveValue>();
    public List<SpawnConditionDataSaveValue> finishCondition = new List<SpawnConditionDataSaveValue>();

    public bool MeetsAppearConditions()
    {
        foreach (SpawnConditionDataSaveValue con in appearCondition)
        {
            if (!con.IsTrue())
                return false;
        }
        return true;
    }

    public bool IsFinished()
    {
        foreach (SpawnConditionDataSaveValue con in finishCondition)
        {
            if (!con.IsTrue())
                return false;
        }
        return true;
    }

    public string GetTitle()
    {
        return title;
    }
}

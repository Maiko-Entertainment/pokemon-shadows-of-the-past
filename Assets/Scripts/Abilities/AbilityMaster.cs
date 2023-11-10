using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMaster : MonoBehaviour
{
    public static AbilityMaster Instance;

    protected Dictionary<string, AbilityData> abilityDictionary = new Dictionary<string, AbilityData>();

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            LoadDictionary();
        }
    }

    public static AbilityMaster GetInstance()
    {
        return Instance;
    }

    public void LoadDictionary()
    {
        AbilityData[] baseDatas = Resources.LoadAll<AbilityData>(ResourceMaster.Instance.GetAbilityDataPath());
        foreach (AbilityData ad in baseDatas)
        {
            abilityDictionary.Add(ad.GetId(), ad);
        }
    }

    public AbilityData GetAbility(string id)
    {
        return abilityDictionary[id];
    }
}

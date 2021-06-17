using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMaster : MonoBehaviour
{
    public static AbilityMaster Instance;

    public List<AbilityData> abilities = new List<AbilityData>();

    protected Dictionary<AbilityId, AbilityData> abilityDictionary = new Dictionary<AbilityId, AbilityData>();

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
        foreach (AbilityData ad in abilities)
        {
            abilityDictionary.Add(ad.abilityId, ad);
        }
    }

    public AbilityData GetAbility(AbilityId id)
    {
        return abilityDictionary[id];
    }
}

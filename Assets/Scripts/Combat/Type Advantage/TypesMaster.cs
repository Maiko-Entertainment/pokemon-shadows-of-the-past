using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TypesMaster: MonoBehaviour
{
    public TypeData nullType;
    public static TypesMaster Instance { get; set; }

    protected Dictionary<string, TypeData> typesData = new Dictionary<string, TypeData>();

    private void Awake()
    {
        Instance = this;
        InstantiateDatabase();
    }

    public void InstantiateDatabase()
    {
        TypeData[] typesDataList = Resources.LoadAll<TypeData>(ResourceMaster.Instance.GetPokemonTypesPath());
        foreach (TypeData typeData in typesDataList)
        {
            typesData.Add(typeData.GetId(), typeData);
        }

    }

    public BattleTypeAdvantageType GetTypeRelation(
        string damageType,
        string defenderType
    )
    {
        if (typesData.ContainsKey(damageType))
        {
            foreach (BattleTypeAdvantageRelation tr in typesData[damageType].effectivenessAgainstTypes)
            {
                if (tr.GetTypeId() == defenderType)
                    return tr.advantageType;
            }
        }
        return BattleTypeAdvantageType.normal;
    }

    public float GetAdvantageMultiplier(BattleTypeAdvantageType advantage)
    {
        switch (advantage)
        {
            case BattleTypeAdvantageType.superEffective:
                return 2f;
            case BattleTypeAdvantageType.resists:
                return 0.5f;
            case BattleTypeAdvantageType.inmune:
                return 0f;
        }
        return 1f;
    }

    public TypeData GetTypeData(string typeId)
    {
        if (typesData.ContainsKey(typeId))
            return typesData[typeId];
        return null;
    }

    public TypeData GetTypeDataNone()
    {
        return nullType;
    }
}

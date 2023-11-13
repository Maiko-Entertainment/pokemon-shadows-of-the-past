using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BattleTypeAdvantageRelation
{
    public TypeData typeData;
    public BattleTypeAdvantageType advantageType;

    public string GetTypeId()
    {
        return typeData.GetId();
    }

}

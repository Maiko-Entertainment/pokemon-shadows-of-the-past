using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Type")]
public class TypeData : ScriptableObject
{
    public string id;
    public string typeName;
    public Color color;
    public Sprite icon;
    public List<BattleTypeAdvantageRelation> effectivenessAgainstTypes = new List<BattleTypeAdvantageRelation>();

    public string GetId()
    {
        return id;
    }
}

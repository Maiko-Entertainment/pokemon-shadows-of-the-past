using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Type")]
public class TypeData : ScriptableObject
{
    public string typeName;
    public PokemonTypeId typeId;
    public Color color;
    public Sprite icon;
    public List<BattleTypeAdvantageRelation> typesRelations = new List<BattleTypeAdvantageRelation>();
}

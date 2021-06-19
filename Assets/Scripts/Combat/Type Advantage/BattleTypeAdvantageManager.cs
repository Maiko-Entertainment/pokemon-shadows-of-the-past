using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BattleTypeAdvantageManager
{
    public List<TypeData> advantages = new List<TypeData>();

    public BattleTypeAdvantageType GetTypeRelation(
        PokemonTypeId damageType,
        PokemonTypeId defenderType
    )
    {
        foreach (TypeData t in advantages)
        {
            if (t.typeId == defenderType)
            {
                foreach (BattleTypeAdvantageRelation tr in t.typesRelations)
                {
                    if (tr.type == damageType)
                        return tr.advantageType;
                }
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

    public TypeData GetTypeData(PokemonTypeId pokemonType)
    {
        foreach(TypeData type in advantages)
        {
            if (type.typeId == pokemonType)
                return type;
        }
        return null;
    }
}

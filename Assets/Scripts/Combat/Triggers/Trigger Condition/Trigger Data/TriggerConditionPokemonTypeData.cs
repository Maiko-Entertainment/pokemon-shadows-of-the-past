using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Trigger Condition/Pokemon Type")]
public class TriggerConditionPokemonTypeData : TriggerConditionData
{
    public List<TypeData> isAtLeastOneOfTypes = new List<TypeData>();

    public override bool MeetsConditions(PokemonBattleData pokemon)
    {
        bool meetsTypeCondition = UtilsMaster.ContainsAtLeastOne(isAtLeastOneOfTypes, pokemon.GetTypes()) || isAtLeastOneOfTypes.Count == 0 && base.MeetsConditions(pokemon);
        meetsTypeCondition = invertCondition ? !meetsTypeCondition : meetsTypeCondition;
        return meetsTypeCondition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityData : ScriptableObject
{
    public AbilityId abilityId;
    public string abilityName;
    public string description;

    // Replaced for each Ability
    public virtual void Initialize(PokemonBattleData pokemon)
    {

    }

    public string GetName()
    {
        return abilityName;
    }
    public string GetDescription()
    {
        return description;
    }
}

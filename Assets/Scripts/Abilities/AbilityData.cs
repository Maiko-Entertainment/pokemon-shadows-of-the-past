using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityData : ScriptableObject
{
    public AbilityId abilityId;

    // Replaced for each Ability
    public virtual void Initialize(PokemonBattleData pokemon)
    {

    }
}

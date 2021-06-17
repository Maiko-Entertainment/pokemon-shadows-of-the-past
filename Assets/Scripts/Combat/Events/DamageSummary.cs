using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSummary
{
    public PokemonTypeId damageType;
    public int damageAmount;
    public DamageSummarySource damageSource;
    // Used to identify the ID of the source, 
    // for instace moveID, abilityId, StatusID
    public int sourceId;

    public DamageSummary(
        PokemonTypeId damageType, 
        int damageAmount, 
        DamageSummarySource damageSource, 
        int sourceId)
    {
        this.damageType = damageType;
        this.damageAmount = damageAmount;
        this.damageSource = damageSource;
        this.sourceId = sourceId;
    }
}

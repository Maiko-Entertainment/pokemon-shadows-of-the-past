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
    // Optional
    public BattleTypeAdvantageType advantageType;
    public PokemonBattleData pokemonSource;

    public DamageSummary(
        PokemonTypeId damageType, 
        int damageAmount,
        DamageSummarySource damageSource, 
        int sourceId,
        BattleTypeAdvantageType advantageType = BattleTypeAdvantageType.normal,
        PokemonBattleData pokemonSource=null)
    {
        this.damageType = damageType;
        this.damageAmount = damageAmount;
        this.damageSource = damageSource;
        this.sourceId = sourceId;
        this.advantageType = advantageType;
        this.pokemonSource = pokemonSource;
    }

    public override string ToString()
    {
        return damageType + " - " + damageAmount + " - " + damageSource + " - " + sourceId;
    }
}

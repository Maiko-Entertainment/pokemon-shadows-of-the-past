using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSummary
{
    public int amount;
    public HealSource source;
    public string sourceId;
    public PokemonBattleData pokemonSource;

    public HealSummary(int amount, HealSource source, string sourceId)
    {
        this.amount = amount;
        this.source = source;
        this.sourceId = sourceId;
    }
}

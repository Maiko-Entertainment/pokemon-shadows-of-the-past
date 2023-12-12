using System.Collections.Generic;

[System.Serializable]
public class BattleStatsGetter
{
    public PokemonBattleStatsMultiplier statMultiplier = new PokemonBattleStatsMultiplier();
    public List<TriggerConditionData> affectConditions = new List<TriggerConditionData>();

    public PokemonBattleStats GetPokemonBattleStats(PokemonBattleData pkmn, PokemonBattleStats modifiedStats)
    {
        if (IsApplicable(pkmn, modifiedStats))
        {
            return Apply(pkmn, modifiedStats);
        }
        else
        {
            return modifiedStats;
        }
    }

    public virtual bool IsApplicable(PokemonBattleData pkmn, PokemonBattleStats stats)
    {
        foreach(TriggerConditionData cond in affectConditions)
        {
            if (!cond.MeetsConditions(pkmn))
            {
                return false;
            }
        }
        return true;
    }

    public virtual PokemonBattleStats Apply(PokemonBattleData pkmn, PokemonBattleStats modifiedStats)
    {
        PokemonBattleStats newBattleStats = statMultiplier.Multiply(modifiedStats);
        return newBattleStats;
    }
}

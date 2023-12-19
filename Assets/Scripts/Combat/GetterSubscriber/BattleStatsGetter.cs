using System.Collections.Generic;

[System.Serializable]
public class BattleStatsGetter
{
    public PokemonBattleStatsMultiplier statMultiplier = new PokemonBattleStatsMultiplier();
    public List<TriggerConditionData> affectConditions = new List<TriggerConditionData>();

    protected List<PokemonBattleData> pokemonFilterList = new List<PokemonBattleData>();

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
        if (pokemonFilterList.Count > 0 && !pokemonFilterList.Contains(pkmn))
        {
            return false;
        }
        foreach (TriggerConditionData cond in affectConditions)
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

    public virtual BattleStatsGetter ApplyToPokemon(PokemonBattleData pokemon)
    {
        BattleStatsGetter battleStatsGetter = Clone();
        battleStatsGetter.PokemonFilterList = new List<PokemonBattleData> { pokemon };
        return battleStatsGetter;
    }

    public List<PokemonBattleData> PokemonFilterList { get { return pokemonFilterList; } set { pokemonFilterList = value; } }

    public virtual BattleStatsGetter Clone()
    {
        BattleStatsGetter stat = new BattleStatsGetter();
        stat.statMultiplier = statMultiplier.Copy();
        stat.affectConditions = affectConditions;
        return stat;
    }
}

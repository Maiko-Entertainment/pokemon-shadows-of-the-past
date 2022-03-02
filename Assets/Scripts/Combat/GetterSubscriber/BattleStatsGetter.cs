using System.Collections.Generic;

[System.Serializable]
public class BattleStatsGetter
{
    public float speedMultiplier = 1f;
    public List<PokemonTypeId> affectedTypes = new List<PokemonTypeId>();

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
        foreach(PokemonTypeId type in pkmn.GetTypeIds())
        {
            if (affectedTypes.Contains(type))
            {
                return true;
            }
        }
        return false;
    }

    public virtual PokemonBattleStats Apply(PokemonBattleData pkmn, PokemonBattleStats modifiedStats)
    {
        PokemonBattleStats newStats = new PokemonBattleStats();
        newStats.accuracy = (int)(modifiedStats.accuracy);
        newStats.evasion = (int)(modifiedStats.evasion);
        newStats.attack = (int)(modifiedStats.attack);
        newStats.spAttack = (int)(modifiedStats.spAttack);
        newStats.defense = (int)(modifiedStats.defense);
        newStats.spDefense = (int)(modifiedStats.spDefense);
        newStats.speed = (int)(modifiedStats.speed * speedMultiplier);
        return newStats;
    }
}

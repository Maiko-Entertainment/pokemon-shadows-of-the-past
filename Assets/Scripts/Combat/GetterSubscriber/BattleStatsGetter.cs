using System.Collections.Generic;

[System.Serializable]
public class BattleStatsGetter
{
    public float speedMultiplier = 1f;
    public List<TypeData> affectedTypes = new List<TypeData>();

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
        foreach(TypeData type in pkmn.GetTypes())
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

[System.Serializable]
public class PokemonBattleStatsMultiplier
{
    public float attack = 1f;
    public float defense = 1f;
    public float spAttack = 1f;
    public float spDefense = 1f;
    public float speed = 1f;

    public PokemonBattleStatsMultiplier Copy()
    {
        PokemonBattleStatsMultiplier newInstance = new PokemonBattleStatsMultiplier();
        newInstance.attack = attack;
        newInstance.defense = defense;
        newInstance.spAttack = spAttack;
        newInstance.spDefense = spDefense;
        newInstance.speed = speed;
        return newInstance;
    }

    public PokemonBattleStats Multiply(PokemonBattleStats statsToMultiply)
    {
        PokemonBattleStats stats = statsToMultiply.Copy();
        stats.attack = (int)(stats.attack * attack);
        stats.defense = (int)(stats.defense * defense);
        stats.spAttack = (int)(stats.spAttack * spAttack);
        stats.spDefense = (int)(stats.spDefense * spDefense);
        stats.speed = (int)(stats.speed * speed);
        return stats;
    }
}

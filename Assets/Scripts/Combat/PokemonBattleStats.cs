﻿[System.Serializable]
public class PokemonBattleStats
{
    public int attack = 0;
    public int defense = 0;
    public int spAttack = 0;
    public int spDefense = 0;
    public int speed = 0;
    public int accuracy = 0;
    public int evasion = 0;
    public int critical = 0;

    public PokemonBattleStats Copy()
    {
        PokemonBattleStats newInstance = new PokemonBattleStats();
        newInstance.attack = attack;
        newInstance.defense = defense;
        newInstance.spAttack = spAttack;
        newInstance.spDefense = spDefense;
        newInstance.speed = speed;
        newInstance.accuracy = accuracy;
        newInstance.evasion = evasion;
        newInstance.critical = critical;
        return newInstance;
    }
}

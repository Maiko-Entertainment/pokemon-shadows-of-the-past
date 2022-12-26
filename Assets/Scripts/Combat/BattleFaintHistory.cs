using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BattleFaintHistory
{
    public PokemonBattleData pokemon;
    public int turn = 0;

    public BattleFaintHistory(PokemonBattleData pokemon, int turn)
    {
        this.pokemon = pokemon;
        this.turn = turn;
    }
}

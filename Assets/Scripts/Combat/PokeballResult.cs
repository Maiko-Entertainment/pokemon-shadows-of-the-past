using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeballResult
{
    public bool wasCaptured;
    public int shakes;
    public PokemonBattleData pokemon;

    public PokeballResult(bool wasCaptured, int shakes, PokemonBattleData pokemon)
    {
        this.wasCaptured = wasCaptured;
        this.shakes = shakes;
        this.pokemon = pokemon;
    }
}

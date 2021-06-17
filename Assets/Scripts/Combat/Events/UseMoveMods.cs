using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to modify moves data by trigger without modifying the Scriptable object
public class UseMoveMods
{
    public float powerMultiplier = 1;
    public float accuracyMultiplier = 1;
    public int criticalLevelChange = 0;
    public PokemonTypeId moveTypeId;

    public UseMoveMods(PokemonTypeId moveTypeId)
    {
        this.moveTypeId = moveTypeId;
    }
}

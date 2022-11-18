using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
// Used to modify moves data by trigger without modifying the Scriptable object
public class UseMoveMods
{
    public float powerMultiplier = 1f;
    public float accuracyMultiplier = 1f;
    public int criticalLevelChange = 0;
    public PokemonTypeId moveTypeId;

    public UseMoveMods(PokemonTypeId moveTypeId)
    {
        this.moveTypeId = moveTypeId;
    }

    public void Implement(UseMoveMods mod)
    {
        powerMultiplier *= mod.powerMultiplier;
        accuracyMultiplier *= mod.accuracyMultiplier;
        criticalLevelChange += mod.criticalLevelChange;
        moveTypeId = mod.moveTypeId == PokemonTypeId.Unmodify ? moveTypeId : mod.moveTypeId;
    }
}

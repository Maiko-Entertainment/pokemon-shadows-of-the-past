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
    public TypeData moveType;

    public UseMoveMods(TypeData moveType)
    {
        this.moveType = moveType;
    }

    public void Implement(UseMoveMods mod)
    {
        powerMultiplier *= mod.powerMultiplier;
        accuracyMultiplier *= mod.accuracyMultiplier;
        criticalLevelChange += mod.criticalLevelChange;
        // null means it doesnt modify it
        moveType = mod.moveType == null ? moveType : mod.moveType;
    }

    public UseMoveMods Clone()
    {
        UseMoveMods copy = new UseMoveMods(moveType);
        copy.powerMultiplier = powerMultiplier;
        copy.accuracyMultiplier = accuracyMultiplier;
        copy.criticalLevelChange = criticalLevelChange;
        return copy;
    }
}

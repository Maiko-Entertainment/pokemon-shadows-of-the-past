using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BattleTriggerDamageMods
{
    public float damageMultiplier = 1f;
    public bool applyEndure = false;

    public BattleTriggerDamageMods(float damageMultiplier, bool applyEndure = false)
    {
        this.damageMultiplier = damageMultiplier;
        this.applyEndure = applyEndure;
    }
}

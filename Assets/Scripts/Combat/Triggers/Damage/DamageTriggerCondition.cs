using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
// Used in the editor to connect moveMods with conditions
public class DamageTriggerCondition
{
    public BattleTriggerDamageMods damageMods = new BattleTriggerDamageMods(1f);
    public TriggerCondition triggerCondition = new TriggerCondition();
    public bool targetDamageTakenInstead = false;
}

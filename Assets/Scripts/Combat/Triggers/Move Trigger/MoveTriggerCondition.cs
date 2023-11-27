using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MoveTriggerCondition
{
    public UseMoveMods moveMods = new UseMoveMods(null);
    public bool cancelMoveInstead = false;
    public TriggerConditionMove triggerCondition = new TriggerConditionMove();
}

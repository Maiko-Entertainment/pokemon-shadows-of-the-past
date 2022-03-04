using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StatusBonus
{
    public StatusEffectId statusToCheck = StatusEffectId.None;
    public float lifeBelowTreshold = 1f;
    // None checks for weather
    public MoveTarget target;
    public float accuracyBonusAdd = 0f;
    public float powerMultiplier = 0;
}

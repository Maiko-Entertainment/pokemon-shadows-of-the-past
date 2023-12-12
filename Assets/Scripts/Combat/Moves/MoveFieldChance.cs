using System;
using UnityEngine;

[Serializable]
public class MoveFieldChance
{
    public StatusFieldData status;
    [Range(0f, 1f)]
    public float chance = 1f;
    public bool removeStatusInstead = false;
}

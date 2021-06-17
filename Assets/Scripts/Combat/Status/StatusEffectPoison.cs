using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectPoison : StatusEffect
{
    public float porcentualDamage = 0.125f;

    public StatusEffectPoison()
    {
        effectId = StatusEffectId.Poison;
        isPrimary = true;
        minTurns = 99999;
    }

    public override void Initiate()
    {

    }
}

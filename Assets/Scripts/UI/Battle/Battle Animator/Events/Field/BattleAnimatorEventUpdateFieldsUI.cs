using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventUpdateFieldsUI : BattleAnimatorEvent
{
    protected List<StatusField> _statusFields;
    public BattleAnimatorEventUpdateFieldsUI(List<StatusField> fieldStatus)
    {
        _statusFields = new List<StatusField>(fieldStatus);
    }

    public override void Execute()
    {
        float time = BattleAnimatorMaster.GetInstance().UpdateFieldStatus(_statusFields);
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim();
    }
}

using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class StatusField : Status
{
    protected StatusFieldData _fieldData;
    protected PokemonBattleData _pokemonSource; // Can be null

    public StatusField(StatusFieldData status, Flowchart flowchartInstance, List<BattleStatsGetter> battleStatsGetters = null) : base(flowchartInstance)
    {
        _fieldData = status;
        _statsGetters = battleStatsGetters;
    }

    public override void Initiate()
    {
        base.Initiate();
        _fieldData.HandleVisualStart(this);
    }

    public virtual void HandleOwnRemove()
    {
        BattleMaster.GetInstance().GetCurrentBattle().AddEvent(new BattleEventFieldStatusRemove(_fieldData));
    }

    public override void PassRound()
    {
        if (IsOver())
        {
            HandleOwnRemove();
        }
        else
        {
            turnsLeft -= 1;
        }
    }

    public StatusFieldData FieldData { get { return _fieldData; } }
}

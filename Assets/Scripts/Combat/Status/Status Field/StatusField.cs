using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusField : Status
{
    protected StatusFieldData _fieldData;
    protected PokemonBattleData _pokemonSource; // Can be null

    public StatusField(StatusFieldData status, Flowchart flowchartInstance): base(flowchartInstance)
    {
        _fieldData = status;
    }
}

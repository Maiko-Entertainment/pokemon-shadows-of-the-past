using System;
using UnityEngine;
using Fungus;
using System.Collections.Generic;

[CommandInfo(
    "Save",
    "Load basic variables for all uses",
    ""
)]

public class FungusLoadBasicVariables : Command
{
    public override void OnEnter()
    {
        List<PokemonCaughtData> party = PartyMaster.GetInstance()?.GetParty();
        for(int i = 0; i < party.Count; i++)
        {
            if (GetFlowchart().GetVariable("partyName" + i)) GetFlowchart().SetStringVariable("partyName" + i, party[i].GetName());
            if (GetFlowchart().GetVariable("partySpecies" + i)) GetFlowchart().SetStringVariable("partySpecies" + i, party[i].pokemonBase.species);
        }
        LoadAllSaveVariables();
        Continue();
    }

    public void LoadAllSaveVariables()
    { ;
        SaveFile mySave = SaveMaster.Instance.GetActiveSave();
        foreach(ObjectElement pse in mySave.elements)
        {
            if (!GetFlowchart().HasVariable(pse.name))
            {
                continue;
            }
            dynamic se = SaveMaster.Instance.GetElement(pse.name);
            switch (se.GetType())
            {
                case bool:
                    GetFlowchart().SetBooleanVariable(pse.name, se);
                    break;
                case int:
                    GetFlowchart().SetIntegerVariable(pse.name, se);
                    break;
                case float:
                    GetFlowchart().SetFloatVariable(pse.name, se);
                    break;
                case string:
                    GetFlowchart().SetStringVariable(se.GetId(), se);
                    break;
            }
        }
    }

    public override Color GetButtonColor()
    {
        return new Color32(42, 176, 49, 255);
    }

    public override string GetSummary()
    {
        return base.GetSummary();
    }
}

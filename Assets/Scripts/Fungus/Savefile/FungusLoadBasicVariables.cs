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
    {
        SaveFile mySave = SaveMaster.Instance.GetActiveSave();
        foreach(PersistedSaveElement pse in mySave.persistedElements)
        {
            if (!GetFlowchart().HasVariable(pse.GetId()))
                continue;
            SaveElement se = SaveMaster.Instance.GetSaveElementData(pse.GetId());
            switch (se.GetValueType())
            {
                case SaveValueType.number:
                    GetFlowchart().SetFloatVariable(se.GetId(), (float)se.GetValue());
                    break;
                default:
                    GetFlowchart().SetStringVariable(se.GetId(), (string)se.GetValue());
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

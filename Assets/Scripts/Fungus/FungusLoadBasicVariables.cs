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
            GetFlowchart().SetStringVariable("partyName" + i, party[i].GetName());
            GetFlowchart().SetStringVariable("partySpecies" + i, party[i].pokemonBase.species);
        }
        SaveElement playerName = SaveMaster.Instance.GetSaveElement(SaveElementId.playerName);
        GetFlowchart().SetStringVariable(SaveElementId.playerName.ToString(), playerName.GetValue().ToString());
        Continue();
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

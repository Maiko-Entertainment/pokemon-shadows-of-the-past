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
        string varPlayerName = SaveElementId.playerName.ToString();
        string varStoryProgressName = SaveElementId.storyProgress.ToString();
        SaveElement playerName = SaveMaster.Instance.GetSaveElement(SaveElementId.playerName);
        SaveElement storyProgress = SaveMaster.Instance.GetSaveElement(SaveElementId.storyProgress);
        if (GetFlowchart().GetVariable(varPlayerName)) GetFlowchart().SetStringVariable(varPlayerName, playerName.GetValue().ToString());
        if (GetFlowchart().GetVariable(varStoryProgressName)) GetFlowchart().SetFloatVariable(varStoryProgressName, (float)storyProgress.GetValue());
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

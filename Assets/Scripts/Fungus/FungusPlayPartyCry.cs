using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo(
    "Audio",
    "Play cry from party member",
    "Play cry from party member by getting the index of it."
)]
public class FungusPlayPartyCry : Command
{
    public AudioOptions clip = new AudioOptions();
    public int partyIndex = 0;
    public override void OnEnter()
    {
        Continue();
        PokemonCaughtData pokemon = PartyMaster.GetInstance().GetParty()[partyIndex];
        if (pokemon != null)
        {
            AudioOptions clipMod = clip.Clone();
            clipMod.audio = pokemon.GetCry();
            AudioMaster.GetInstance()?.PlaySfx(clipMod);

        }
    }

    public override Color GetButtonColor()
    {
        return new Color32(42, 176, 49, 255);
    }
    public override string GetSummary()
    {
        string clipName =  "Party Index: " + partyIndex + " - Pitch: " + clip.pitch;
        return clipName;
    }
}

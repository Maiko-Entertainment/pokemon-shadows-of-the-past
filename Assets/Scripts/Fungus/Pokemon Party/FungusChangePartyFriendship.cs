using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Game",
    "Change party friendship",
    "Change the friendship value of the pokemon in the party."
)]
public class FungusChangePartyFriendship : Command
{
    public int change = 40;
    public override void OnEnter()
    {
        List<PokemonCaughtData> party =  PartyMaster.GetInstance().GetParty();
        foreach(PokemonCaughtData pkmn in party)
        {
            pkmn.GainFriendship(change);
        }
        Continue();
    }
}

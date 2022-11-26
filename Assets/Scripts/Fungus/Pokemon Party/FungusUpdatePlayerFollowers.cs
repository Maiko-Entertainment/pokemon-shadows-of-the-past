using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Game",
    "Update Player Followers",
    "Updates the pokefollowers of the player."
)]
public class FungusUpdatePlayerFollowers : Command
{
    public override void OnEnter()
    {
        PlayerController player = WorldMapMaster.GetInstance().GetPlayer();
        player?.UpdatePokeFollower();
        Continue();
    }
}

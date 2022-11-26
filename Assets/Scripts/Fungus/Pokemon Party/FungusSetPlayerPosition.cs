using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Game",
    "Set Players Position",
    "Sets the current players position."
)]
public class FungusSetPlayerPosition : Command
{
    public Transform position;
    public override void OnEnter()
    {
        PlayerController player = WorldMapMaster.GetInstance().GetPlayer();
        player?.SetPosition(position.position);
        Continue();
    }
}

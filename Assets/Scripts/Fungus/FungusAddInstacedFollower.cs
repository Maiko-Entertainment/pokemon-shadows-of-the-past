using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Game",
    "Add Instanced Follower",
    "Adds a follower to the player that has already been instanced."
)]
public class FungusAddInstacedFollower : Command
{
    public WorldInteractableBrainFollower follower;
    public bool repeatable = false;
    public override void OnEnter()
    {
        WorldMapMaster.GetInstance()?.AddInstancedFollowerToPlayer(follower, repeatable);
        Continue();
    }
}

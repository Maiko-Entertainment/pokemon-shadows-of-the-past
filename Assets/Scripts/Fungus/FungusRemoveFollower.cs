using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Game",
    "Remove Follower",
    "Removes a follower from the player."
)]
public class FungusRemoveFollower : Command
{
    public string followerId = "Tansy";
    public bool alsoDestroy = false;
    public bool addToCurrentCutscene = true;

    public override void OnEnter()
    {
        WorldInteractableBrainFollower follower = WorldMapMaster.GetInstance()?.RemoveFollowerFromPlayer(followerId);
        if (follower)
        {
            if (addToCurrentCutscene)
            {
                CutsceneMaster.GetInstance().GetCurrentCutscene()?.cutsceneAgents.Add(follower);
            }
            else if (alsoDestroy)
            {
                Destroy(follower.gameObject);
            }
        }
        Continue();
    }
}

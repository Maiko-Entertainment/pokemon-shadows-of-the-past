using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Game",
    "Update Quest",
    "Updates the quest master to show current active quests."
)]
public class FungusUpdateQuest : Command
{
    public override void OnEnter()
    {
        UIQuestMaster.Instance?.LoadCurrentQuest();
        Continue();
    }
}

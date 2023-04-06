using Fungus;
using System.Collections;
using UnityEngine;

[CommandInfo(
    "World",
    "Go To World Map",
    ""
)]
public class FungusGoToWorldMap : Command
{
    public int mapId = 0;
    public int spawnIndex = 0;
    public ViewTransition changeMapTransition;
    public bool triggerNextInteractionOnEnd = true;

    public override void OnEnter()
    {
        float changeTime = 0;
        TransitionMaster.GetInstance().ClearTransitions();
        if (changeMapTransition)
        {
            TransitionMaster.GetInstance().RunTransition(changeMapTransition);
            changeTime = changeMapTransition.changeTime;
        }
        StartCoroutine(RunEvent(changeTime));
    }

    IEnumerator RunEvent(float delay)
    {
        yield return new WaitForSeconds(delay);
        WorldMapMaster.GetInstance().GoToMap(mapId, spawnIndex);
        if (triggerNextInteractionOnEnd)
        {
            InteractionsMaster.GetInstance().ExecuteNext();
        }
        else
        {
            Continue();
        }
    }

    public override Color GetButtonColor()
    {
        return new Color32(42, 176, 49, 255);
    }

    public override string GetSummary()
    {
        return "Map ID: "+mapId + " - Spawn Index: "+spawnIndex;
    }
}

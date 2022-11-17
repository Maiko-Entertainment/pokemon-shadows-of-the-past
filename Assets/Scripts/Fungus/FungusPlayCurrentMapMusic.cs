using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Audio",
    "Play Current Map Music",
    "Plays music current map music."
)]
public class FungusPlayCurrentMapMusic : Command
{
    public override void OnEnter()
    {
        AudioOptions mapMusic = WorldMapMaster.GetInstance().GetCurrentMap().GetMapMusic();
        AudioMaster.GetInstance()?.PlayMusic(mapMusic);
        Continue();
    }

    public override Color GetButtonColor()
    {
        return new Color32(42, 176, 49, 255);
    }
}

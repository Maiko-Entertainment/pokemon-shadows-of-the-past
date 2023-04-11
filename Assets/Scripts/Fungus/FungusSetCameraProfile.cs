using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[CommandInfo(
    "Game",
    "Set Camera Profile",
    "Set's a new camera profile, empty for reset."
)]
public class FungusSetCameraProfile : Command
{
    public VolumeProfile profile;
    public bool affectWorld = true;
    public bool affectBattle = true;

    public override void OnEnter()
    {
        if (affectWorld)
            TransitionMaster.GetInstance().SetCameraProfile(profile);
        if (affectBattle)
            TransitionMaster.GetInstance().SetBattleCameraProfile(profile);
        Continue();
    }
}

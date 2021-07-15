using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventPlaySound : BattleAnimatorEvent
{
    public AudioClip clip;

    public BattleAnimatorEventPlaySound(AudioClip soundEffect, bool dontWait = false): base(dontWait)
    {
        clip = soundEffect;
    }

    public override void Execute()
    {
        AudioMaster.GetInstance()?.PlaySfx(clip);
        base.Execute();
    }
}

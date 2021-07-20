using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventPlaySound : BattleAnimatorEvent
{
    public AudioClip clip;
    public float pitch = 1;

    public BattleAnimatorEventPlaySound(AudioClip soundEffect, float pitch = 1f, bool dontWait = false): base(dontWait)
    {
        clip = soundEffect;
        this.pitch = pitch;
    }

    public override void Execute()
    {
        AudioMaster.GetInstance()?.PlaySfx(clip, pitch);
        if (!dontWait)
        {

            BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim(clip!=null ? clip.length : 0);
        }
        base.Execute();
    }
}

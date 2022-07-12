using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventPlaySound : BattleAnimatorEvent
{
    public AudioClip clip;
    public float pitch = 1;
    public AudioOptions audioOptions;

    public BattleAnimatorEventPlaySound(AudioClip soundEffect, float pitch = 1f, bool dontWait = false): base(dontWait)
    {
        clip = soundEffect;
        this.pitch = pitch;
    }
    public BattleAnimatorEventPlaySound(AudioOptions audioOptions, bool dontWait = false) : base(dontWait)
    {
        this.audioOptions = audioOptions;
    }

    public override void Execute()
    {
        if (audioOptions != null)
        {
            AudioMaster.GetInstance()?.PlaySfx(audioOptions);
        }
        else
        {
            AudioMaster.GetInstance()?.PlaySfx(clip, pitch);
        }
        if (!dontWait)
        {
            BattleAnimatorMaster.GetInstance()?.GoToNextBattleAnim(clip!=null ? clip.length : 0);

        }
        base.Execute();
    }
}

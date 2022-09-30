using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AudioOptions
{
    public AudioClip audio;
    public float pitch = 1f;
    public float customDuration = -1f;
    public float volumeModifier = 1f;
    public float fadeTime = 0.5f;
    public bool loopMusic = true;

    public AudioOptions() { }
    public AudioOptions(AudioClip audio, float pitch = 1f, bool loopMusic = true)
    {
        this.audio = audio;
        this.pitch = pitch;
        this.loopMusic = loopMusic;
    }

    public AudioOptions Clone()
    {
        AudioOptions clone = new AudioOptions(audio, pitch, loopMusic);
        clone.customDuration = customDuration;
        clone.volumeModifier = volumeModifier;
        clone.fadeTime = fadeTime;
        return clone;
    }
}

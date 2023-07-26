using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMaster : MonoBehaviour
{
    public static AudioMaster Instance;

    public float soundEffectVolume = 1;
    public float musicVolume = 1;

    public float musicConstantVolume = 0.75f;
    public float soundConstantVolume = 0.75f;

    public AudioMixerGroup effectsMixer;

    public AudioSource musicSource;
    public List<AudioSource> voiceSources = new List<AudioSource>();
    public AudioSource voiceSource;

    protected AudioOptions currentClip;
    protected AudioOptions nextClip;
    public float fadeTimeTotal = 0;
    public float currentFadeTime = 0;

    void Awake()
    {
        if (Instance)
        {
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        SetSoundEffectVolume(soundEffectVolume);
    }

    public static AudioMaster GetInstance()
    {
        return Instance;
    }

    public void HandleSave()
    {
        SaveMaster.Instance.activeSaveFile.musicVolume = musicVolume;
        SaveMaster.Instance.activeSaveFile.soundVolume = soundEffectVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume * musicConstantVolume;
    }

    public float GetSoundVolume()
    {
        return soundEffectVolume * soundConstantVolume;
    }

    // Plays a sound effect once
    public void PlaySfx(AudioClip clip, float pitch=1f)
    {
        float duration = clip!=null ? clip.length : 0f;
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = GetSoundVolume();
        audioSource.pitch = pitch;
        audioSource.outputAudioMixerGroup = effectsMixer;
        audioSource.Play();
        Destroy(audioSource, duration / pitch + 0.5f);
    }
    public void PlaySfx(AudioOptions sound)
    {
        AudioClip clip = sound.audio;
        float duration = clip != null && sound.customDuration <= 0 ? clip.length : sound.customDuration;
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = GetSoundVolume() * sound.volumeModifier;
        audioSource.pitch = sound.pitch;
        audioSource.outputAudioMixerGroup = effectsMixer;
        audioSource.Play();
        Destroy(audioSource, duration / sound.pitch + 0.1f);
    }

    internal void Load(SaveFile save)
    {
        musicVolume = save.musicVolume;
        soundEffectVolume = save.soundVolume;
    }

    public void PlaySfxInAudioSource(AudioOptions sound, AudioSource customSource)
    {
        AudioClip clip = sound.audio;
        customSource.clip = clip;
        customSource.volume = GetSoundVolume() * sound.volumeModifier;
        customSource.pitch = sound.pitch;
        customSource.Play();
    }
    public void PlaySfxWithDuration(AudioClip clip, float duration, float pitch = 1f)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = GetSoundVolume();
        audioSource.pitch = pitch;
        audioSource.Play();
        Destroy(audioSource, duration);
    }
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.volume = GetMusicVolume();
            musicSource.pitch = 1f;
            musicSource.Play();
        }
    }
    public void PlayMusic(AudioOptions clip)
    {
        if (clip.fadeTime > 0)
        {
            nextClip = clip.Clone();
            fadeTimeTotal = nextClip.fadeTime;
            currentFadeTime = nextClip.fadeTime;
        }
        else 
        {
            musicSource.volume = clip.volumeModifier * GetMusicVolume();
            musicSource.pitch = clip.pitch;
            musicSource.loop = clip.loopMusic;
            currentClip = clip;
            if (musicSource.clip != clip.audio)
            {
                musicSource.clip = clip.audio;
                musicSource.Play();
            }
        }
    }

    public void StopMusic(bool fade = false)
    {
        musicSource.Stop();
    }

    public void SetSoundEffectVolume(float volume)
    {
        soundEffectVolume = volume;
        foreach (AudioSource audioSource in voiceSources)
            audioSource.volume = GetSoundVolume();
        if (voiceSource)
        {
            voiceSource.volume = 0.5f * volume;
        }
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    private void Update()
    {
        if (currentFadeTime > 0)
            currentFadeTime = Mathf.Max(currentFadeTime - Time.deltaTime, 0);
        if (currentFadeTime > fadeTimeTotal / 2)
        {
            if (musicSource.clip)
            {
                musicSource.volume = GetMusicVolume() * currentClip.volumeModifier * Mathf.Max(currentFadeTime, fadeTimeTotal / 2) / fadeTimeTotal / 2;
            }
            else
            {
                currentFadeTime = fadeTimeTotal / 2;
            }
        }
        else if (currentFadeTime <= fadeTimeTotal / 2)
        {
            if (nextClip != null)
            {
                nextClip.fadeTime = 0;
                PlayMusic(nextClip);
                nextClip = null;
            }
            if (currentClip != null && currentClip.audio)
            {
                musicSource.volume = GetMusicVolume() * currentClip.volumeModifier * (fadeTimeTotal - currentFadeTime * 2) / fadeTimeTotal;
            }
        }
    }
}

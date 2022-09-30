using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
    public static AudioMaster Instance;

    public float soundEffectVolume = 1;
    public float musicVolume = 1;

    public AudioSource musicSource;
    public List<AudioSource> voiceSources = new List<AudioSource>();

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

    // Plays a sound effect once
    public void PlaySfx(AudioClip clip, float pitch=1f)
    {
        float duration = clip!=null ? clip.length : 0f;
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = soundEffectVolume;
        audioSource.pitch = pitch;
        audioSource.Play();
        Destroy(audioSource, duration + 0.5f);
    }
    public void PlaySfx(AudioOptions sound)
    {
        AudioClip clip = sound.audio;
        float duration = clip != null && sound.customDuration <= 0 ? clip.length : sound.customDuration;
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = soundEffectVolume * sound.volumeModifier;
        audioSource.pitch = sound.pitch;
        audioSource.Play();
        Destroy(audioSource, duration + 0.1f);
    }
    public void PlaySfxInAudioSource(AudioOptions sound, AudioSource customSource)
    {
        AudioClip clip = sound.audio;
        customSource.clip = clip;
        customSource.volume = soundEffectVolume * sound.volumeModifier;
        customSource.pitch = sound.pitch;
        customSource.Play();
    }
    public void PlaySfxWithDuration(AudioClip clip, float duration, float pitch = 1f)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = soundEffectVolume;
        audioSource.pitch = pitch;
        audioSource.Play();
        Destroy(audioSource, duration);
    }
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.volume = musicVolume;
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
            musicSource.volume = clip.volumeModifier * musicVolume;
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
            audioSource.volume = volume;
    }

    private void Update()
    {
        if (currentFadeTime > 0)
            currentFadeTime = Mathf.Max(currentFadeTime - Time.deltaTime, 0);
        if (currentFadeTime > fadeTimeTotal / 2)
        {
            if (musicSource.clip)
            {
                musicSource.volume = musicVolume * currentClip.volumeModifier * Mathf.Max(currentFadeTime, fadeTimeTotal / 2) / fadeTimeTotal / 2;
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
                musicSource.volume = musicVolume * currentClip.volumeModifier * (fadeTimeTotal - currentFadeTime * 2) / fadeTimeTotal;
            }
        }
    }
}

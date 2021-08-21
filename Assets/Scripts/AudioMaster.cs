using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
    public static AudioMaster Instance;

    public float soundEffectVolume = 1;
    public float musicVolume = 1;

    public AudioSource musicSource;

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
            musicSource.Play();
        }
    }

    public void StopMusic(bool fade)
    {
        musicSource.Stop();
    }
}

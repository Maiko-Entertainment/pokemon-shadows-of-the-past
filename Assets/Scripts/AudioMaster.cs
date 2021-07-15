using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
    public static AudioMaster Instance;

    public float soundEffectVolume = 1;

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
    public void PlaySfx(AudioClip clip)
    {
        float duration = clip.length;
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = soundEffectVolume;
        audioSource.Play();
        Destroy(audioSource, duration + 0.5f);
    }
}

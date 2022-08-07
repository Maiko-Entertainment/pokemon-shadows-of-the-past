using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapDamageDealerInterval : WorldMapDamageDealer
{
    public float safeInterval = 3f;
    public float damageInterval = 0.5f;
    public float intervalInitialValue = 0f;
    public float prewarningTime = 1f;
    public Animator animator;

    public AudioOptions onWarningSound;
    public AudioOptions OnDamageActiveSound;

    public AudioSource audioSource;

    protected float timePassed = 0;
    protected bool hasWarned = false;

    private void Start()
    {
        timePassed = intervalInitialValue;
        StartCoroutine(InitiateInterval());
    }

    IEnumerator InitiateInterval()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (!InteractionsMaster.GetInstance().IsInteractionPlaying())
            {
                timePassed += Time.deltaTime;
                float intervalTouse = isDamageActive ? damageInterval : safeInterval;
                if (!isDamageActive && (intervalTouse - prewarningTime) < timePassed && !hasWarned)
                {
                    hasWarned = true;
                    animator?.SetTrigger("Warn");
                    AudioMaster.GetInstance().PlaySfx(onWarningSound);
                }
                if (intervalTouse < timePassed)
                {
                    timePassed = 0;
                    isDamageActive = !isDamageActive;
                    hasWarned = false;
                    if (isDamageActive)
                    {
                        animator?.SetTrigger("Attack");
                        AudioMaster.GetInstance().PlaySfxInAudioSource(OnDamageActiveSound, audioSource);
                    }
                    else
                    {
                        animator?.SetTrigger("Idle");
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapDamageDealerEnterArea : WorldMapDamageDealer
{
    public float damageInterval = 0.5f;
    public float prewarningTime = 1f;
    public Animator animator;

    public AudioOptions onWarningSound;
    public AudioOptions OnDamageActiveSound;

    public AudioSource audioSource;

    protected float timePassed = 0;

    private void Start()
    {

    }

    public void TriggerEnterArea()
    {
        StartCoroutine(InitiateInterval());
    }

    IEnumerator InitiateInterval()
    {
        if (!InteractionsMaster.GetInstance().IsInteractionPlaying())
        {
            isDamageActive = false;
            animator?.SetTrigger("Warn");
            AudioMaster.GetInstance().PlaySfx(onWarningSound);
            yield return new WaitForSeconds(prewarningTime);
            AudioMaster.GetInstance().PlaySfx(OnDamageActiveSound);
            isDamageActive = true;
            yield return new WaitForSeconds(damageInterval);
            isDamageActive = false;
            animator?.SetTrigger("Idle");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterChangeLightSettings : MonoBehaviour
{
    public LightSettings settings;
    public float fadeTime = 2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerLight.Instance && collision.tag == "Player")
        {
            PlayerLight.Instance.FadeToSettings(settings, fadeTime);
        }
    }
}

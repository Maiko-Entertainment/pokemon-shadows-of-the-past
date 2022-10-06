using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    public Light2D lightComponent;
    public static PlayerLight Instance;

    public float fadeTime = 0;
    public float fadeTimeTotal = 0;
    protected LightSettings initialSettings;
    protected LightSettings fadeToSettings;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            if (lightComponent)
                initialSettings = new LightSettings(lightComponent);
            fadeToSettings = initialSettings;
        }
    }

    public void FadeToSettings(LightSettings settings, float fadeTime)
    {
        initialSettings = fadeToSettings;
        fadeToSettings = settings;
        this.fadeTime = fadeTime;
        fadeTimeTotal = fadeTime;
    }

    private void Update()
    {
        if (lightComponent && fadeTimeTotal > 0)
        {
            fadeTime = Mathf.Max(fadeTime - Time.deltaTime, 0);
            lightComponent.color = Color.Lerp(initialSettings.color, fadeToSettings.color, 1 - fadeTime / fadeTimeTotal);
            lightComponent.intensity = Mathf.Lerp(initialSettings.intensity, fadeToSettings.intensity, 1 - fadeTime / fadeTimeTotal);
            lightComponent.pointLightInnerRadius = Mathf.Lerp(
                initialSettings.pointLightInnerRadius,
                fadeToSettings.pointLightInnerRadius,
                1 - fadeTime / fadeTimeTotal);
            lightComponent.pointLightOuterRadius = Mathf.Lerp(
                initialSettings.pointLightOuterRadius,
                fadeToSettings.pointLightOuterRadius,
                1 - fadeTime / fadeTimeTotal);
        }
    }
}

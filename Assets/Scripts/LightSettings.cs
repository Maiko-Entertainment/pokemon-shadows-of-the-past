using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class LightSettings
{
    public Color color;
    public float intensity = 1;
    public float pointLightInnerRadius;
    public float pointLightOuterRadius = 8;
    public float falloffIntensity = 0.7f;

    public LightSettings(Light2D light)
    {
        color = light.color;
        intensity = light.intensity;
        pointLightInnerRadius = light.pointLightInnerRadius;
        pointLightOuterRadius = light.pointLightOuterRadius;
        falloffIntensity = light.falloffIntensity;
    }

    public LightSettings(
        Color color,
        float intensity,
        float pointLightInnerRadius,
        float pointLightOuterRadius,
        float falloffIntensity = 0.5f
        )
    {
        this.color = color;
        this.intensity = intensity;
        this.pointLightInnerRadius = pointLightInnerRadius;
        this.pointLightOuterRadius = pointLightOuterRadius;
        this.falloffIntensity = falloffIntensity;
    }
}

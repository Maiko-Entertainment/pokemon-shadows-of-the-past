﻿using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleHealthbar : MonoBehaviour
{
    public TextMeshProUGUI pokemonName;
    public TextMeshProUGUI pokemonLevel;
    public Image currentBar;
    public Image currentExpBar;
    public Image statusSimbol;
    public TextMeshProUGUI currenthealth;
    public float changeSpeed = 0.2f;
    public Color healthyColor;
    public Color dangerColor;
    public Color criticalColor;
    public TransitionBase abilityPanel;
    public TextMeshProUGUI pokemonAbilityName;
    public AudioClip abilitySound;

    public PokemonBattleData pokemon;
    public float targetHealth;
    private float currentValue;
    private float maxHealth;

    public float targetExp;
    private float currentExp;
    private int maxExp;

    public void Load(PokemonBattleData pokemon)
    {
        this.pokemon = pokemon;
        PokemonCaughtData pkmn = pokemon.GetPokemonCaughtData();
        currentValue = pkmn.GetCurrentHealth();
        maxHealth = pkmn.GetCurrentStats().health;
        currentExp = pkmn.GetExperience();
        pokemonName.text = pkmn.pokemonName;
        pokemonLevel.text = "Lv. " + pkmn.GetLevel();
        pokemonAbilityName.text = AbilityMaster.GetInstance().GetAbility(pkmn.abilityId).GetName();
        UpdateHealth(currentValue);
        UpdateTarget(currentValue);
        UpdateExp(currentExp);
        UpdateTargetExp(currentExp, pkmn.GetTotalExperienceToNextLevel());
        StatusEffect currenStatus = pokemon.GetCurrentPrimaryStatus();
        StatusEffectData status = BattleAnimatorMaster.GetInstance().GetStatusEffectData(currenStatus != null ? currenStatus.effectId : StatusEffectId.None);
        UpdateStatus(status);
    }

    public void FadeIn()
    {
        GetComponent<TransitionFade>()?.FadeIn();
    }
    public void FadeOut()
    {
        GetComponent<TransitionFade>()?.FadeOut();
    }

    public float UpdateTarget(float target)
    {
        targetHealth = target;
        float time = Mathf.Abs(targetHealth - currentValue) / GetChangeSpeed();
        return time;
    }

    public void UpdateHealth(float value)
    {
        currentValue = value;
        currentBar.fillAmount = value / maxHealth;
        if (currentBar.fillAmount > 0.5f)
            currentBar.color = healthyColor;
        else if (currentBar.fillAmount > 0.25f)
            currentBar.color = dangerColor;
        else
            currentBar.color = criticalColor;
        if (currenthealth)
        {
            currenthealth.text = "" + (int)value + "/" + maxHealth;
        }
    }

    public float UpdateTargetExp(float target, int max)
    {
        targetExp = target;
        UpdateMaxExp(max);
        float time = Mathf.Abs(targetExp - currentExp) / GetExpChangeSpeed();
        return time;
    }
    public void UpdateMaxExp(int maxExp)
    {
        this.maxExp = maxExp;
    }
    public void UpdateExp(float value)
    {
        currentExp = value;
        if (currentExpBar)
        {
            if (currentExpBar.fillAmount == 1)
            {
                currentExp = 0;
                UpdateTargetExp(0, maxExp);
            }
            currentExpBar.fillAmount = currentExp / maxExp;
        }
    }

    public void UpdateStatus(StatusEffectData status)
    {
        if (status)
        {
            statusSimbol.enabled = true;
            statusSimbol.sprite = status.icon;
            return;
        }
        statusSimbol.enabled = false;
    }
    public void UpdateLevel(int level)
    {
        pokemonLevel.text = "Lv. " + level;
    }

    private void Update()
    {
        if (currentValue != targetHealth)
        {
            currentValue = Mathf.MoveTowards(
                currentValue, targetHealth,
                GetChangeSpeed() * Time.deltaTime);
            UpdateHealth(currentValue);
        }
        if (currentExp != targetExp)
        {
            currentExp = Mathf.MoveTowards(
                currentExp, targetExp,
                GetExpChangeSpeed() * Time.deltaTime);
        }
        UpdateExp(currentExp);
    }

    public float GetChangeSpeed()
    {
        return maxHealth * changeSpeed;
    }

    public float GetExpChangeSpeed()
    {
        return maxExp * changeSpeed * 2f;
    }

    public float ShowAbility()
    {
        float stayTime = 2f;
        abilityPanel.FadeIn();
        AudioMaster.GetInstance()?.PlaySfx(abilitySound);
        StartCoroutine(FadeOutAbiliy(stayTime));
        return stayTime;
    }

    IEnumerator FadeOutAbiliy(float time)
    {
        yield return new WaitForSeconds(time);
        abilityPanel.FadeOut();
    }

}

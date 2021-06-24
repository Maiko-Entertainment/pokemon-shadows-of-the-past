using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleHealthbar : MonoBehaviour
{
    public TextMeshProUGUI pokemonName;
    public TextMeshProUGUI pokemonLevel;
    public Image currentBar;
    public Image statusSimbol;
    public TextMeshProUGUI currenthealth;
    public float changeSpeed = 0.2f;
    public Color healthyColor;
    public Color dangerColor;
    public Color criticalColor;

    public PokemonBattleData pokemon;
    public float targetHealth;
    private float currentValue;
    private float maxHealth;

    public void Load(PokemonBattleData pokemon)
    {
        this.pokemon = pokemon;
        PokemonCaughtData pkmn = pokemon.GetPokemonCaughtData();
        currentValue = pkmn.GetCurrentHealth();
        maxHealth = pkmn.GetCurrentStats().health;
        pokemonName.text = pkmn.pokemonName;
        pokemonLevel.text = "Lv. " + pkmn.GetLevel();
        UpdateHealth(currentValue);
        UpdateTarget(currentValue);
        StatusEffect currenStatus = pokemon.GetCurrentPrimaryStatus();
        StatusEffectData status = BattleAnimatorMaster.GetInstance().GetStatusEffectData(currenStatus != null ? currenStatus.effectId : StatusEffectId.None);
        UpdateStatus(status);
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

    private void Update()
    {
        if (currentValue != targetHealth)
        {
            currentValue = Mathf.MoveTowards(
                currentValue, targetHealth,
                GetChangeSpeed() * Time.deltaTime);
            UpdateHealth(currentValue);
        }
    }

    public float GetChangeSpeed()
    {
        return maxHealth * changeSpeed;
    }
}

using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIBattleHealthbar : MonoBehaviour
{
    public TextMeshProUGUI pokemonName;
    public TextMeshProUGUI pokemonLevel;
    public Image currentBar;
    public Image currentExpBar;
    public Image statusSimbol;
    public Image caughtIcon;
    public TextMeshProUGUI currenthealth;
    public float changeSpeed = 0.2f;
    public Color healthyColor;
    public Color dangerColor;
    public Color criticalColor;
    public TransitionBase abilityPanel;
    public TextMeshProUGUI pokemonAbilityName;
    public AudioClip abilitySound;
    public Transform minorStatusList;
    public UIPokemonGender genderIcon;
    public UIStatusMinor statusMinorPrefab;
    public UIStatChange statChangePrefab;

    public PokemonBattleData pokemon;
    public float targetHealth;
    private float currentValue;
    private float maxHealth;

    public float targetExp;
    private float currentExp;
    private int maxExp;

    public void Load(PokemonBattleData pokemon)
    {
        bool isCaught = PokemonMaster.GetInstance().GetPokemonPokedexData(pokemon.GetPokemonCaughtData().GetPokemonBaseData().pokemonId).caughtAmount > 0;
        this.pokemon = pokemon;
        PokemonCaughtData pkmn = pokemon.GetPokemonCaughtData();
        currentValue = pkmn.GetCurrentHealth();
        maxHealth = pkmn.GetCurrentStats().health;
        currentExp = pkmn.GetExperience();
        pokemonName.text = pkmn.GetName();
        pokemonLevel.text = "Lv. " + (pokemon.hideLevel ? "???" : pkmn.GetLevel());
        pokemonAbilityName.text = pkmn.ability.GetName();
        UpdateHealth(currentValue);
        UpdateTarget(currentValue);
        UpdateExp(currentExp);
        UpdateTargetExp(currentExp, pkmn.GetTotalExperienceToNextLevel());
        StatusEffect currenStatus = pokemon.GetCurrentPrimaryStatus();
        StatusEffectData status = currenStatus !=null ? currenStatus.effectData : null;
        List<StatusEffect> statusEffectsMinor = pokemon.GetNonPrimaryStatus();
        List<StatusEffectData> statusDatas = new List<StatusEffectData>();
        foreach(StatusEffect s in statusEffectsMinor)
        {
            statusDatas.Add(s.effectData);
        }
        PokemonBaseData baseData = pkmn.GetPokemonBaseData();
        if (baseData.isGenderless || pokemon.hideLevel)
        {
            genderIcon.gameObject.SetActive(false);
        }
        else
        {
            genderIcon.gameObject.SetActive(true);
            genderIcon.Load(pkmn.isMale);
        }
        if (caughtIcon)
        {
            caughtIcon.enabled = isCaught;
        }
        UpdateStatus(status, statusDatas);
        UpdateStats(pokemon);
    }

    public void FadeIn()
    {
        GetComponent<TransitionFade>()?.FadeIn();
    }
    public void FadeOut(bool instant = false)
    {
        if (instant)
        {
            GetComponent<TransitionFade>()?.Hide();
        }
        else
        {
            GetComponent<TransitionFade>()?.FadeOut();
        }
    }

    public void LoadStatus(List<StatusEffectData> status)
    {
        foreach (Transform t in minorStatusList)
        {
            if (t.GetComponent<UIStatusMinor>())
            {
                StatusEffectData s = t.GetComponent<UIStatusMinor>().status;
                if (!status.Contains(s))
                {
                    Destroy(t.gameObject);
                }
            }
        }
        foreach (StatusEffectData sed in status)
        {
            bool contains = false;
            foreach(Transform t in minorStatusList)
            {
                if (t.GetComponent<UIStatusMinor>())
                {
                    StatusEffectData s = t.GetComponent<UIStatusMinor>().status;
                    if (sed == s)
                    {
                        contains = true;
                    }
                }
            }
            if (!contains)
            {
                Instantiate(statusMinorPrefab, minorStatusList).Load(sed);
                AudioMaster.GetInstance().PlaySfx(abilitySound);
            }
        }
    }
    public void UpdateStats(PokemonBattleData pokemonState)
    {
        foreach (Transform t in minorStatusList)
        {
            if (t.GetComponent<UIStatChange>())
            {
                Destroy(t.gameObject);
            }
        }
        PokemonBattleStats battleStats = pokemonState.GetBattleStatsChangeLevels();
        HandleStatUpDown("Atk", battleStats.attack);
        HandleStatUpDown("Def", battleStats.defense);
        HandleStatUpDown("Sp. Atk", battleStats.spAttack);
        HandleStatUpDown("Sp. Def", battleStats.spDefense);
        HandleStatUpDown("Spd", battleStats.speed);
        HandleStatUpDown("Eva", battleStats.evasion);
        HandleStatUpDown("Acc", battleStats.accuracy);
        HandleStatUpDown("Crt", battleStats.critical);
    }

    public void HandleStatUpDown(string statName, int change)
    {
        if (change != 0)
        {
            Instantiate(statChangePrefab, minorStatusList).Load(statName, change);
        }
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

    public void UpdateStatus(StatusEffectData status, List<StatusEffectData> minor)
    {
         if (status)
        {
            statusSimbol.enabled = true;
            statusSimbol.sprite = status.icon;
            AudioMaster.GetInstance().PlaySfx(abilitySound);
        }
        else
        {
            statusSimbol.enabled = false;
        }
        LoadStatus(minor);
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

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemOptionsPokemon : MonoBehaviour, ISelectHandler
{
    public Image pokemonIcon;
    public TextMeshProUGUI pokemonName;
    public Image healthbar;
    public TextMeshProUGUI healthValue;
    public TextMeshProUGUI level;
    public Image exphbar;
    public Image statusSimbol;
    public Image itemIcon;
    public TransitionFade selectedArrow;
    public TransitionFade swapIcon;
    public Color faintedColor;
    public Animator animator;

    public delegate void Hover(PokemonCaughtData pkmn);
    public event Hover onHover;
    public delegate void Click(PokemonCaughtData pkmn);
    public event Click onClick;

    public PokemonCaughtData pokemon;
    private float targetHealth;
    private float currentHealth;

    public UIItemOptionsPokemon Load(PokemonCaughtData pokemon)
    {
        this.pokemon = pokemon;
        pokemonIcon.sprite = pokemon.GetIcon();
        pokemonName.text = pokemon.GetName();
        pokemonName.color = pokemon.IsFainted() ? faintedColor : Color.white;
        float currentHealth = pokemon.GetCurrentHealth();
        UpdateHealth(currentHealth);
        level.text = "Lv " + pokemon.level;
        if (exphbar)
            exphbar.fillAmount = ((float)pokemon.GetExperience()) / pokemon.GetTotalExperienceToNextLevel();
        if (statusSimbol)
        {
            StatusEffectData status = BattleAnimatorMaster.GetInstance().GetStatusEffectData(pokemon.statusEffectId);
            UpdateStatus(status);
        }
        if (pokemon.equippedItem)
        {
            itemIcon.gameObject.SetActive(true);
            itemIcon.sprite = pokemon.equippedItem.icon;
        }
        else
        {
            itemIcon.gameObject.SetActive(false);
            itemIcon.sprite = null;
        }
        targetHealth = currentHealth;
        return this;
    }

    public void UpdateHealth(float health)
    {
        float maxHealth = pokemon.GetCurrentStats().health;
        currentHealth = health;
        healthValue.text = ((int)health) + "/" + maxHealth;
        healthbar.fillAmount = health / maxHealth;
    }
    public void SetTargetToCurrent()
    {
        targetHealth = pokemon.GetCurrentHealth();
    }

    public void UpdateStatus(StatusEffect currenStatus)
    {
        StatusEffectData status = BattleAnimatorMaster.GetInstance().GetStatusEffectData(currenStatus != null ? currenStatus.effectId : StatusEffectId.None);
        UpdateStatus(status);
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
    public void UpdateSelected(PokemonCaughtData pkmn)
    {
        if (pokemon == pkmn)
        {
            animator.SetTrigger("Highlighted");
            selectedArrow?.FadeIn();
        }
        else
        {
            animator.SetTrigger("Normal");
            selectedArrow?.FadeOut();
        }
    }
    public void UpdateSwaping(PokemonCaughtData pkmn)
    {
        if (pokemon == pkmn)
            swapIcon?.FadeIn();
        else
            swapIcon?.FadeOut();
    }
    public void UpdateHealth()
    {
        targetHealth = pokemon.GetCurrentHealth();
    }

    public void HandleClick()
    {
        onClick?.Invoke(pokemon);
    }

    public void HandleHover()
    {
        onHover?.Invoke(pokemon);
    }

    public void HandleHit()
    {
        animator?.SetTrigger("Hit");
    }

    private void Update()
    {
        if (targetHealth != currentHealth)
        {
            float nextHealth = Mathf.MoveTowards(currentHealth, targetHealth, pokemon.GetCurrentStats().health * 0.3f * Time.deltaTime);
            UpdateHealth(nextHealth);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        HandleHover();
    }
}

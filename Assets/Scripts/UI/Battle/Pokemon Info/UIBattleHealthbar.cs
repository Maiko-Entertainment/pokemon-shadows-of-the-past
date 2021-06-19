using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleHealthbar : MonoBehaviour
{
    public TextMeshProUGUI pokemonName;
    public Image currentBar;
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
        UpdateHealth(pokemon, currentValue);
        UpdateTarget(currentValue);
    }

    public float UpdateTarget(float target)
    {
        targetHealth = target;
        float time = Mathf.Abs(targetHealth - currentValue) / GetChangeSpeed();
        return time;
    }

    public void UpdateHealth(PokemonBattleData pokemon, float value)
    {
        PokemonCaughtData pkmn = pokemon.GetPokemonCaughtData();
        
        pokemonName.text = pkmn.pokemonName;
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

    private void Update()
    {
        if (currentValue != targetHealth)
        {
            currentValue = Mathf.MoveTowards(
                currentValue, targetHealth,
                GetChangeSpeed() * Time.deltaTime);
            UpdateHealth(pokemon, currentValue);
        }
    }

    public float GetChangeSpeed()
    {
        return maxHealth * changeSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPokemonInfo : MonoBehaviour
{
    public UIStat statPrefab;

    public Transform statsContainer;

    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI abilityDescription;

    public void Load(PokemonCaughtData pokemon)
    {
        PokemonBaseStats stats = pokemon.GetCurrentStats();
        PokemonNatureId nature = pokemon.natureId;
        Instantiate(statPrefab, statsContainer).GetComponent<UIStat>().Load("Attack", ""+stats.attack, nature == PokemonNatureId.ruthless);
        Instantiate(statPrefab, statsContainer).GetComponent<UIStat>().Load("Defense", "" + stats.defense, nature == PokemonNatureId.careful);
        Instantiate(statPrefab, statsContainer).GetComponent<UIStat>().Load("Sp. Attack", "" + stats.spAttack, nature == PokemonNatureId.cunning);
        Instantiate(statPrefab, statsContainer).GetComponent<UIStat>().Load("Sp. Defense", "" + stats.spDefense, nature == PokemonNatureId.reserved);
        Instantiate(statPrefab, statsContainer).GetComponent<UIStat>().Load("Speed", "" + stats.speed, nature == PokemonNatureId.restless);
        AbilityData ability = AbilityMaster.GetInstance().GetAbility(pokemon.abilityId);
        abilityName.text = ability.GetName();
        abilityDescription.text = ability.GetDescription();
    }
}

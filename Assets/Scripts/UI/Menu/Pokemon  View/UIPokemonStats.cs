using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPokemonStats : MonoBehaviour
{
    public UIStat statPrefab;

    public Transform statsContainer;

    public TextMeshProUGUI title;
    public TextMeshProUGUI natureDescription;

    public void Load(PokemonCaughtData pokemon)
    {
        bool isShadow = pokemon.isShadow;
        PokemonBaseStats stats = pokemon.GetCurrentStats();
        PokemonNatureId nature = pokemon.natureId;
        Instantiate(statPrefab, statsContainer).GetComponent<UIStat>().Load("Attack", "" + stats.attack, nature == PokemonNatureId.ruthless);
        Instantiate(statPrefab, statsContainer).GetComponent<UIStat>().Load("Defense", "" + stats.defense, nature == PokemonNatureId.careful);
        Instantiate(statPrefab, statsContainer).GetComponent<UIStat>().Load("Sp. Attack", "" + stats.spAttack, nature == PokemonNatureId.cunning);
        Instantiate(statPrefab, statsContainer).GetComponent<UIStat>().Load("Sp. Defense", "" + stats.spDefense, nature == PokemonNatureId.reserved);
        Instantiate(statPrefab, statsContainer).GetComponent<UIStat>().Load("Speed", "" + stats.speed, nature == PokemonNatureId.restless);
        title.text = "Nature: ";
        if (isShadow)
        {
            natureDescription.text = "Raises all stats by 20% and gains ";
        }
        else
        {
            natureDescription.text = "Raises ";
        }
        switch (nature)
        {
            case PokemonNatureId.ruthless:
                title.text += isShadow ? "Feral" : "Ruthless";
                natureDescription.text += "Attack";
                break;
            case PokemonNatureId.careful:
                title.text += isShadow ? "Reclusive" : "Careful";
                natureDescription.text += "Defense";
                break;
            case PokemonNatureId.cunning:
                title.text += isShadow ? "Selfish" : "Cunning";
                natureDescription.text += "Sp. Attack";
                break;
            case PokemonNatureId.reserved:
                title.text += isShadow ? "Paranoid" : "Reserved";
                natureDescription.text += "Sp. Defense";
                break;
            case PokemonNatureId.restless:
                title.text += isShadow ? "Nervous" : "Restless";
                natureDescription.text += "Speed";
                break;
        }
        natureDescription.text += " equal to half the pokemon's level (Rounded up).";
    }
}

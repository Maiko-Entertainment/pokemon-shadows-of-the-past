using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPokedexEvolution : MonoBehaviour
{
    public TextMeshProUGUI condition;
    public Image icon;

    public UIPokedexEvolution Load(PokemonBaseEvolution evoData, bool hasSeen, bool hasSeenEvo)
    {
        PokemonBaseData evo = evoData.pokemon;
        icon.sprite = evo.GetIcon();
        if (hasSeenEvo)
        {
            icon.color = Color.white;
        }
        else
        {
            icon.color = Color.black;
        }
        if (!hasSeen)
        {
            condition.text = "No Data";
        }
        else
        {
            string requirements = "";
            int index = 0;
            foreach(PokemonEvolutionDataRequirement req in evoData.evolutionRequirements)
            {
                requirements += req.GetRequirementText();
                index++;
                if (index + 1 < evoData.evolutionRequirements.Count)
                {
                    requirements += ", ";
                }
                else if (index < evoData.evolutionRequirements.Count)
                {
                    requirements += " & ";
                }
            }
            condition.text = requirements;
        }
        return this;
    }
}

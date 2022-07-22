using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPokedexAbility : MonoBehaviour
{
    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI abilityChance;

    public void Load(string ability, string percentage)
    {
        abilityName.text = ability;
        abilityChance.text = percentage;
    }
}

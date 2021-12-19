using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPokemonInfo : MonoBehaviour
{
    public UIStat statPrefab;

    public Transform statsContainer;

    public Image itemSprite;
    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI abilityDescription;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;


    public void Load(PokemonCaughtData pokemon)
    {
        ItemDataOnPokemon item = pokemon.GetEquippedItem();
        if (item)
        {
            itemSprite.gameObject.SetActive(true);
            itemSprite.sprite = item.icon;
            itemName.text = item.GetName();
            itemDescription.text = item.GetDescription();
        }
        else
        {
            itemSprite.gameObject.SetActive(false);
            itemSprite.sprite = null;
            itemName.text = "None";
            itemDescription.text = "To equip your pokemon with an item go to the items menu.";
        }
        AbilityData ability = AbilityMaster.GetInstance().GetAbility(pokemon.abilityId);
        abilityName.text = ability.GetName();
        abilityDescription.text = ability.GetDescription();
    }
}

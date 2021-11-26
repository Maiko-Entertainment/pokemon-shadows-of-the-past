using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemsViewer : MonoBehaviour
{
    public UIItemsView itemPrefab;
    public UIItemOptionsPokemon pokemonPrefab;
    public AudioClip sectionChangeSound;

    public Transform itemsContainer;
    public Transform pokemonListContainer;

    public Color selectedItemColor;
    public TransitionBase itemInfo;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Image itemIcon;
    public Button useButton;

    private ItemCategory currentCategory;
    private ItemInventory currentItem;
    private PokemonCaughtData currentPokemon;
    private Color defaultItemColor;

    void Start()
    {
        defaultItemColor = itemInfo.GetComponent<Image>().color;
        Load();
    }

    public void Load()
    {
        LoadPokemonList();
        ViewCategory(ItemCategory.General, true);
    }

    public void Clean()
    {
        foreach(Transform i in itemsContainer)
        {
            i.GetComponent<UIItemsView>().onClick -= ViewItem;
            Destroy(i.gameObject);
        }
    }

    public void CleanPokemon()
    {
        currentPokemon = null;
        foreach (Transform i in pokemonListContainer)
        {
            i.GetComponent<UIItemOptionsPokemon>().onClick -= SetCurrentPokemon;
            i.GetComponent<UIItemOptionsPokemon>().onClick -= UseCurrentItemOnPokemon;
            Destroy(i.gameObject);
        }
    }

    public void LoadPokemonList()
    {
        CleanPokemon();
        List<PokemonCaughtData> party = PartyMaster.GetInstance().GetParty();
        foreach(PokemonCaughtData p in party)
        {
            Instantiate(pokemonPrefab, pokemonListContainer).GetComponent<UIItemOptionsPokemon>().Load(p);
        }
    }

    public void ViewCategory(ItemCategory category, bool force=false)
    {
        SetCurrentPokemon(null);
        if (currentCategory != category || force)
        {
            Clean();
            List<ItemInventory> items = InventoryMaster.GetInstance().GetItemsFromCategory(category);
            foreach (ItemInventory i in items)
            {
                if (i.GetAmount() > 0)
                {
                    UIItemsView uiItem = Instantiate(itemPrefab, itemsContainer).GetComponent<UIItemsView>().Load(i);
                    uiItem.onHover += ViewItem;
                    uiItem.onClick += UseItem;
                }
            }
            if (currentCategory != category && !force)
            {
                DeactivatePokemonSelector();
            }
        }
        currentCategory = category;
    }

    public void ViewItem(ItemInventory item)
    {
        ItemData i = item.itemData;
        itemName.text = i.GetName();
        itemDescription.text = i.GetDescription();
        itemIcon.sprite = i.icon;
        if (itemInfo)
        {
            itemInfo?.FadeIn();
            if (currentItem == item)
            {
                itemInfo.GetComponent<Image>().color = selectedItemColor;
            }
            else
            {
                itemInfo.GetComponent<Image>().color = defaultItemColor;
            }
        }
    }

    public void ViewCategoryGeneral()
    {
        if (sectionChangeSound) AudioMaster.GetInstance().PlaySfx(sectionChangeSound);
        ViewCategory(ItemCategory.General);
    }
    public void ViewCategoryBerry()
    {
        if (sectionChangeSound) AudioMaster.GetInstance().PlaySfx(sectionChangeSound);
        ViewCategory(ItemCategory.Berry);
    }
    public void ViewCategoryPokeball()
    {
        if (sectionChangeSound) AudioMaster.GetInstance().PlaySfx(sectionChangeSound);
        ViewCategory(ItemCategory.Pokeball);
    }

    public void UseItem(ItemInventory item)
    {
        ItemData i = item.itemData;

        if (i.CanUse().canUse)
        {
            currentItem = item;
            if (i.GetItemTargetType() == ItemTargetType.Pokemon)
            {
                ItemDataOnPokemon ip = (ItemDataOnPokemon)i;
                ActivatePokemonSelector(ip);
                ViewItem(item);
            }
            else
            {
                i.Use();
                DeactivatePokemonSelector();
                ViewCategory(currentCategory, true);
            }
        }
    }

    public void ActivatePokemonSelector(ItemDataOnPokemon ip)
    {
        CleanPokemon();
        List<PokemonCaughtData> party = PartyMaster.GetInstance().GetParty();
        foreach (PokemonCaughtData p in party)
        {
            UIItemOptionsPokemon pkmn = Instantiate(pokemonPrefab, pokemonListContainer).GetComponent<UIItemOptionsPokemon>().Load(p);
            CanUseResult canUse = ip.CanUseOnPokemon(p);
            if (canUse.canUse && currentPokemon == null)
            {
                currentPokemon = p;
            }
            pkmn.UpdateSelected(currentPokemon);
            pkmn.onClick += UseCurrentItemOnPokemon;
            pkmn.onHover += SetCurrentPokemon;
        }
    }

    public void DeactivatePokemonSelector()
    {
        LoadPokemonList();
        itemInfo?.FadeOut();
    }

    public void UseCurrentItemOnPokemon(PokemonCaughtData pokemon)
    {
        ((ItemDataOnPokemon)currentItem.itemData).UseOnPokemon(currentPokemon);
        ViewCategory(currentCategory, true);
        UpdatePokemonsHealth();
        if (currentItem.GetAmount() == 0)
        {
            DeactivatePokemonSelector();
        }
    }
    public void SetCurrentPokemon(PokemonCaughtData pokemon)
    {
        currentPokemon = pokemon;
        foreach (Transform i in pokemonListContainer)
        {
            i.GetComponent<UIItemOptionsPokemon>().UpdateSelected(currentPokemon);
        }
    }
    public void UpdatePokemonsHealth()
    {
        foreach (Transform i in pokemonListContainer)
        {
            i.GetComponent<UIItemOptionsPokemon>().SetTargetToCurrent();
        }
    }

    public void HandleClose()
    {
        float time = 0;
        TransitionBase transition = GetComponent<TransitionBase>();
        if (transition)
        {
            transition.FadeOut();
            time = 1f / Mathf.Abs(transition.speed);
        }
        Destroy(gameObject, time);
    }
}

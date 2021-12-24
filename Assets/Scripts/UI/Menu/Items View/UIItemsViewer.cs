using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

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
    public Transform sectionContainer;
    public TransitionBase useButton;
    public TransitionBase equipButton;

    private ItemCategory currentCategory;
    private ItemInventory currentItem;
    private PokemonCaughtData currentPokemon;
    private int currentSectionIndex = 0;

    void Start()
    {
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
            Instantiate(pokemonPrefab, pokemonListContainer).Load(p);
        }
    }

    public void ViewCategory(ItemCategory category, bool force=false)
    {
        SetCurrentPokemon(null);
        if (currentCategory != category || force)
        {
            Clean();
            List<ItemInventory> items = InventoryMaster.GetInstance().GetItemsFromCategory(category);
            List<Selectable> elements = new List<Selectable>();
            foreach (ItemInventory i in items)
            {
                if (i.GetAmount() > 0)
                {
                    UIItemsView uiItem = Instantiate(itemPrefab, itemsContainer).Load(i);
                    uiItem.onHover += ViewItem;
                    uiItem.onClick += UseItem;
                    elements.Add(uiItem.GetComponent<Selectable>());
                }
            }
            if (currentCategory != category && !force)
            {
                DeactivatePokemonSelector();
            }
            UtilsMaster.LineSelectables(elements);
            if (elements.Count > 0)
            {
                UtilsMaster.SetSelected(elements[0].gameObject);
            }
        }
        currentCategory = category;
        HandleSectionChange();
    }

    public void ViewItem(ItemInventory item)
    {
        ItemData i = item.itemData;
        itemName.text = i.GetName();
        itemDescription.text = i.GetDescription();
        itemIcon.sprite = i.icon;
        currentItem = item;
        if (itemInfo)
        {
            itemInfo?.FadeIn();
            if (i.CanUse().canUse)
            {
                useButton.FadeIn();
            }
            else
            {
                useButton.FadeOut();
            }
            try
            {
                ItemDataOnPokemon ip = (ItemDataOnPokemon)item.itemData;
                if (ip != null && ip.equipable)
                {
                    equipButton.FadeIn();
                }
                else
                {
                    equipButton.FadeOut();
                }
            }
            catch
            {
                equipButton.FadeOut();
            }
        }
    }
    public void ReturnToItemSelectionList()
    {
        foreach (Transform pokemon in pokemonListContainer)
        {
            if (pokemon.GetComponent<UIItemOptionsPokemon>().pokemon == currentPokemon)
            {
                EventSystem eventSystem = EventSystem.current;
                eventSystem.SetSelectedGameObject(pokemon.gameObject, new BaseEventData(eventSystem));
                break;
            }
        }
    }
    public void HandleItemMenuSection(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            int previousIndex = currentSectionIndex;
            if (direction.x < 0)
            {
                if (currentSectionIndex == 0)
                {
                    return;
                }
                currentSectionIndex = Mathf.Max(0, currentSectionIndex - 1);
                ReturnToItemSelectionList();
            }
            else if (direction.x > 0)
            {
                currentSectionIndex = Mathf.Min(sectionContainer.childCount, currentSectionIndex + 1);
                ReturnToItemSelectionList();
            }
            if (previousIndex != currentSectionIndex)
            {
                ViewCurrentSection();
                HandleSectionChange();
            }
        }
    }
    public void HandleSectionChange()
    {
        foreach (Transform section in sectionContainer)
        {
            Transform selectedSection = sectionContainer.GetChild(currentSectionIndex);
            if (selectedSection == section)
            {
                section.GetComponent<TransitionFade>().FadeIn();
            }
            else
            {
                section.GetComponent<TransitionFade>().FadeOut();
            }
        }
    }
    public void ViewCurrentSection()
    {
        switch (currentSectionIndex)
        {
            case 0:
                ViewCategoryGeneral();
                break;
            case 1:
                ViewCategoryBerry();
                break;
            case 2:
                ViewCategoryPokeball();
                break;
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
    public void HandleEquip(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            ActivatePokemonSelector((ItemDataOnPokemon)currentItem.itemData, true);
        }
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
                ActivatePokemonSelector(ip, false);
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

    public void ActivatePokemonSelector(ItemDataOnPokemon ip, bool equipMode)
    {
        CleanPokemon();
        List<PokemonCaughtData> party = PartyMaster.GetInstance().GetParty();
        List<Selectable> elements = new List<Selectable>();
        foreach (PokemonCaughtData p in party)
        {
            UIItemOptionsPokemon pkmn = Instantiate(pokemonPrefab, pokemonListContainer).GetComponent<UIItemOptionsPokemon>().Load(p);
            CanUseResult canUse = ip.CanUseOnPokemon(p);
            pkmn.UpdateSelected(currentPokemon);
            if (equipMode)
            {
                pkmn.onClick += EquipCurrentItemOnPokemon;
            }
            if (canUse.canUse)
            {
                pkmn.onClick += UseCurrentItemOnPokemon;
            }
            pkmn.onHover += SetCurrentPokemon;
            elements.Add(pkmn.GetComponent<Button>());
        }
        foreach(Transform p in pokemonListContainer)
        {
            UtilsMaster.LineSelectables(elements);
        }
        currentPokemon = party[0];
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(elements[0].gameObject, new BaseEventData(eventSystem));
    }

    public void DeactivatePokemonSelector()
    {
        LoadPokemonList();
        itemInfo?.FadeOut();
    }

    public void UseCurrentItemOnPokemon(PokemonCaughtData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (bm.IsBattleActive())
        {
            PokemonBattleData pbd = bm.GetPokemonFromCaughtData(pokemon);
            BattleTeamId teamId = bm.GetTeamId(pbd);
            BattleMaster.GetInstance().GetCurrentBattle()?
                .HandleTurnInput(new BattleTurnDesitionItemPokemonUse(
                    pbd,
                    (ItemDataOnPokemon) currentItem.itemData,
                    teamId
                ));
            BattleAnimatorMaster.GetInstance().HideOptions();
        }
        else
        {
            ((ItemDataOnPokemon)currentItem.itemData).UseOnPokemon(currentPokemon);
        }
        ViewCategory(currentCategory, true);
        UpdatePokemonsHealth();
        if (currentItem.GetAmount() == 0)
        {
            DeactivatePokemonSelector();
        }
    }
    public void EquipCurrentItemOnPokemon(PokemonCaughtData pokemon)
    {
        pokemon.EquipItem((ItemDataOnPokemon)currentItem.itemData);
        DeactivatePokemonSelector();
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
    public void HandleClose(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            HandleClose();
        }
    }

    public void HandleClose()
    {
        if (currentPokemon != null)
        {
            DeactivatePokemonSelector();
        }
        else if (BattleMaster.GetInstance().IsBattleActive())
        {
            BattleAnimatorMaster.GetInstance().HideItemSelection(true);
        }
        else
        {
            UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
        }
    }
}

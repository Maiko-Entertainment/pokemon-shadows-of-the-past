using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class UIBattleItemPickerManager : MonoBehaviour
{
    public UIBattleItem itemPrefab;
    public UIBattleOptionsPokemon pokemonTargetPrefab;

    public TransitionFade panel;
    public Transform itemList;
    public TransitionFade pokemonList;
    public TextMeshProUGUI itemDescription;

    private PokemonBattleData currentPokemon;
    private ItemCategory currentCategory;
    private bool isSelectingPokemon = false;

    private void Start()
    {
        LoadItemsByCategory(ItemCategory.General);
    }

    public void LoadItemsByCategory(ItemCategory itemCategory)
    {
        currentCategory = itemCategory;
        List<ItemInventory> items = InventoryMaster.GetInstance().GetItemsFromCategory(itemCategory);
        foreach(Transform i in itemList)
        {
            i.GetComponent<UIBattleItem>().onHover -= PreviewItem;
            Destroy(i.gameObject);
        }
        foreach (ItemInventory i in items)
        {
            if (i.HasEnough())
            {
                UIBattleItem item = Instantiate(itemPrefab, itemList).GetComponent<UIBattleItem>().Load(i);
                item.onHover += PreviewItem;
            }
        }
    }

    public void PreviewItem(ItemInventory item)
    {
        itemDescription.text = item.itemData.description;
    }

    public void ShowItemSelector()
    {
        isSelectingPokemon = false;
        panel.FadeIn();
        LoadItemsByCategory(currentCategory);
        HidePokemonListItem();
    }
    public void HideItemSelector()
    {
        panel.FadeOut();
    }

    public void ShowPokemonList(ItemDataOnPokemon item)
    {
        isSelectingPokemon = true;
        List<PokemonBattleData> pokemon = BattleMaster.GetInstance().GetCurrentBattle()?.team1.pokemon;
        HidePokemonListItem();
        foreach (PokemonBattleData pkmn in pokemon)
        {
            UIBattleOptionsPokemon pkmnBattle = Instantiate(pokemonTargetPrefab, pokemonList.transform).Load(pkmn);
            pkmnBattle.onClick += (PokemonBattleData p)=> HandleItemUse(p,item);
            pkmnBattle.onHover += SetCurrentlySelected;
        }
    }

    public void HidePokemonListItem()
    {
        foreach (Transform pkmn in pokemonList.transform)
        {
            Destroy(pkmn.gameObject);
        }
    }
    public void HandleItemUse(PokemonBattleData pkmn, ItemDataOnPokemon item)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTeamId teamId = bm.GetTeamId(pkmn);
        BattleMaster.GetInstance().GetCurrentBattle()?
            .HandleTurnInput(new BattleTurnDesitionItemPokemonUse(
                pkmn,
                item,
                teamId
            ));
        HideItemSelector();
    }
    public void UpdatePokemon()
    {
        foreach (Transform pkmn in pokemonList.transform)
        {
            pkmn.gameObject.GetComponent<UIBattleOptionsPokemon>().UpdateSelected(currentPokemon);
        }
    }

    public void SetCurrentlySelected(PokemonBattleData pkmn)
    {
        currentPokemon = pkmn;
        UpdatePokemon();
    }
    public void HandleCancel(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            if (isSelectingPokemon)
            {
                ShowItemSelector();
            }
            else
            {
                HideItemSelector();
            }
        }
    }
}

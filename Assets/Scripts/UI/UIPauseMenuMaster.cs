using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenuMaster : MonoBehaviour
{
    public static UIPauseMenuMaster Instance;

    public UIMenuPile pokemonViewerPrefab;
    public UIMenuPile itemViewerPrefab;
    public UIMenuPile pokedexViewerPrefab;
    public UIMenuPile shopViewerPrefab;
    public UIMenuPile pcViewerPrefab;
    public UIMenuPile saveViewerPrefab;
    public UIMenuPile optionsViewerPrefab;
    public UIMenuPile mainMenuPrefab;
    public UIItemOptionsPokemon pokemonMiniViewPrefab;
    public GameObject dayPrefab;
    public GameObject nightPrefab;
    public AudioClip menuOpenSound;

    public UIMenuPile menuPrefab;
    public UIMenuConfirm confirmMenuPrefab;
    public Button opener;

    public Transform menuContainer;
    public Transform submenuContainer;
    public Transform pokemonMiniViewList;
    public Transform timeofDayContainer;

    private List<UIMenuPile> openedMenus = new List<UIMenuPile>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        ShowWorldUI();
    }
    public static UIPauseMenuMaster GetInstance() { return Instance; }

    public UIMenuPile OpenMenu(UIMenuPile menuPrefab, bool force = false, bool noSound = false)
    {
        bool isInteracting = InteractionsMaster.GetInstance().IsInteracting();
        if (!isInteracting || force)
        {
            UIMenuPile instance = Instantiate(menuPrefab, submenuContainer);
            if (openedMenus.Count > 0)
            {
                openedMenus[openedMenus.Count - 1].DeactivateMenu();
            }
            openedMenus.Add(instance);
            instance.Open();
            HideMenuOpener();
            if (menuOpenSound && !noSound) AudioMaster.GetInstance().PlaySfx(menuOpenSound);
            return instance;
        }
        return null;
    }
    public void ShowMenuOpener()
    {
        opener.gameObject.SetActive(true);
    }
    public void HideMenuOpener()
    {
        opener.gameObject.SetActive(false);
    }
    public void HideWorldUI()
    {
        HideMenuOpener();
        UIQuestMaster.Instance?.HideQuest();
        pokemonMiniViewList?.gameObject.SetActive(false);
    }
    public void ShowWorldUI()
    {
        ShowMenuOpener();
        UIQuestMaster.Instance?.ShowQuest();
        pokemonMiniViewList?.gameObject.SetActive(true);
        UpdatePartyMiniPreview();
        UpdateTimeOfDay();
    }

    public void OpenMenu()
    {
        if (!IsMenuOpen())
        {
            OpenMenu(menuPrefab);
        }
    }

    public void CloseCurrentMenu()
    {
        if (openedMenus.Count > 0)
        {
            UIMenuPile menu = openedMenus[openedMenus.Count - 1];
            openedMenus.RemoveAt(openedMenus.Count - 1);
            menu.Close();
            if (openedMenus.Count > 0)
            {
                openedMenus[openedMenus.Count - 1].ReactivateMenu();
            }
            else
            {
                ShowMenuOpener();
            }
        }
    }

    public void CloseAllMenus()
    {
        foreach(UIMenuPile menu in openedMenus)
        {
            menu.Close();
        }
        openedMenus = new List<UIMenuPile>();
    }
    public UIMenuPile GetCurrentMenu()
    {
        return openedMenus[openedMenus.Count - 1];
    }
    public void OpenPokedexViewer()
    {
        if (menuOpenSound) AudioMaster.GetInstance().PlaySfx(menuOpenSound);
        OpenMenu(pokedexViewerPrefab);
    }
    public void OpenPokemonViewer()
    {
        if (menuOpenSound) AudioMaster.GetInstance().PlaySfx(menuOpenSound);
        OpenMenu(pokemonViewerPrefab);
    }

    public void OpenItemsViewer()
    {
        if (menuOpenSound) AudioMaster.GetInstance().PlaySfx(menuOpenSound);
        OpenMenu(itemViewerPrefab);
    }

    public void OpenShopMenu(List<ItemData> itemsForSale)
    {
        OpenMenu(shopViewerPrefab, true).GetComponent<UIShopViewer>().Load(itemsForSale);
    }

    public void OpenSettingsViewer()
    {
        if (menuOpenSound) AudioMaster.GetInstance().PlaySfx(menuOpenSound);
        OpenMenu(optionsViewerPrefab);
    }
    public void OpenSaveMenu(bool isMainMenu = false)
    {
        OpenMenu(saveViewerPrefab, true).GetComponent<UISaveFiles>().Load(isMainMenu);
    }

    public void OpenPcMenu()
    {
        OpenMenu(pcViewerPrefab, true).GetComponent<UIPokemonPC>().Load();
    }

    public void OpenMainMenu()
    {
        OpenMenu(mainMenuPrefab, true, true);
    }
    public UIMenuConfirm OpenConfirmMenu(string title, string description, UIMenuConfirm.OnInput onConfirm, UIMenuConfirm.OnInput onDeny = null)
    {
        return OpenMenu(confirmMenuPrefab, true, true).GetComponent<UIMenuConfirm>().Load(title, description, onConfirm, onDeny);
    }

    public bool IsMenuOpen() { return openedMenus.Count > 0; }

    public void UpdatePartyMiniPreview()
    {
        foreach (Transform t in pokemonMiniViewList)
            Destroy(t.gameObject);
        List<PokemonCaughtData> party = PartyMaster.GetInstance().GetParty();
        foreach(PokemonCaughtData p in party)
        {
            UIItemOptionsPokemon pokemonOpt = Instantiate(pokemonMiniViewPrefab, pokemonMiniViewList).Load(p);
            pokemonOpt.GetComponent<Button>().interactable = false;
        }
    }

    public void DoHitPokemonAnim(PokemonCaughtData pokemon)
    {
        foreach (Transform p in pokemonMiniViewList)
        {
            UIItemOptionsPokemon pokemonOpt = p.GetComponent<UIItemOptionsPokemon>();
            pokemonOpt.Load(pokemonOpt.pokemon);
            if (pokemonOpt.pokemon == pokemon)
            {
                pokemonOpt.HandleHit();
            }
        }
    }

    public void UpdateTimeOfDay()
    {
        foreach (Transform t in timeofDayContainer)
            Destroy(t.gameObject);
        TimeOfDayType time = WorldMapMaster.GetInstance().GetTimeOfDay();
        if (time == TimeOfDayType.Day)
        {
            Instantiate(dayPrefab, timeofDayContainer);
        }
        else
        {
            Instantiate(nightPrefab, timeofDayContainer);
        }
    }
}

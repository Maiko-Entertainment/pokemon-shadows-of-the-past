using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenuMaster : MonoBehaviour
{
    public static UIPauseMenuMaster Instance;

    public UIMenuPile pokemonViewerPrefab;
    public UIMenuPile itemViewerPrefab;
    public UIItemOptionsPokemon pokemonMiniViewPrefab;
    public GameObject dayPrefab;
    public GameObject nightPrefab;
    public AudioClip menuOpenSound;

    public UIMenuPile menuPrefab;
    public Button opener;

    public Transform menuContainer;
    public Transform submenuContainer;
    public Transform pokemonMiniViewList;
    public Transform timeofDayContainer;

    public UIVariablesList variablesInstance;

    private bool isMenuOpen = false;
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

    public void OpenMenu(UIMenuPile menuPrefab)
    {
        UIMenuPile instance = Instantiate(menuPrefab, submenuContainer);
        if (openedMenus.Count > 0)
        {
            openedMenus[openedMenus.Count - 1].DeactivateMenu();
        }
        openedMenus.Add(instance);
        instance.Open();
        HideMenuOpener();
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
        pokemonMiniViewList?.gameObject.SetActive(false);
    }
    public void ShowWorldUI()
    {
        ShowMenuOpener();
        pokemonMiniViewList?.gameObject.SetActive(true);
        UpdatePartyMiniPreview();
        UpdateTimeOfDay();
    }

    public void OpenMenu()
    {
        if (menuOpenSound) AudioMaster.GetInstance().PlaySfx(menuOpenSound);
        OpenMenu(menuPrefab);
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
    
    public void OpenSaveViewer()
    {
        if (menuOpenSound) AudioMaster.GetInstance().PlaySfx(menuOpenSound);
        SaveMaster.Instance.Save(0);
        // Instantiate(itemViewerPrefab, submenuContainer);
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

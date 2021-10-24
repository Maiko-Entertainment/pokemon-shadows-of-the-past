using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenuMaster : MonoBehaviour
{
    public static UIPauseMenuMaster Instance;

    public UIPokemonView pokemonViewerPrefab;
    public UIItemsViewer itemViewerPrefab;
    public UIItemOptionsPokemon pokemonMiniViewPrefab;
    public GameObject dayPrefab;
    public GameObject nightPrefab;
    public AudioClip menuOpenSound;

    public TransitionBase menu;
    public GameObject opener;

    public Transform menuContainer;
    public Transform submenuContainer;
    public Transform pokemonMiniViewList;
    public Transform timeofDayContainer;

    private bool isMenuOpen = false;

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

    public void HideWorldUI()
    {
        opener.SetActive(false);
        pokemonMiniViewList?.gameObject.SetActive(false);
    }
    public void ShowWorldUI()
    {
        opener.SetActive(true);
        pokemonMiniViewList?.gameObject.SetActive(true);
        UpdatePartyMiniPreview();
        UpdateTimeOfDay();
    }

    public void OpenMenu()
    {
        if (menuOpenSound) AudioMaster.GetInstance().PlaySfx(menuOpenSound);
        menu.FadeIn();
        isMenuOpen = true;
    }

    public void CloseMenu()
    {
        menu.FadeOut();
        isMenuOpen = false;
    }

    public void OpenPokemonViewer()
    {
        if (menuOpenSound) AudioMaster.GetInstance().PlaySfx(menuOpenSound);
        Instantiate(pokemonViewerPrefab, submenuContainer);
    }

    public void OpenItemsViewer()
    {
        if (menuOpenSound) AudioMaster.GetInstance().PlaySfx(menuOpenSound);
        Instantiate(itemViewerPrefab, submenuContainer);
    }
    
    public void OpenSaveViewer()
    {
        if (menuOpenSound) AudioMaster.GetInstance().PlaySfx(menuOpenSound);
        SaveMaster.Instance.Save(0);
        // Instantiate(itemViewerPrefab, submenuContainer);
    }

    public bool IsMenuOpen() { return isMenuOpen; }

    public void UpdatePartyMiniPreview()
    {
        foreach (Transform t in pokemonMiniViewList)
            Destroy(t.gameObject);
        List<PokemonCaughtData> party = PartyMaster.GetInstance().GetParty();
        foreach(PokemonCaughtData p in party)
        {
            UIItemOptionsPokemon pokemonOpt = Instantiate(pokemonMiniViewPrefab, pokemonMiniViewList).Load(p);
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

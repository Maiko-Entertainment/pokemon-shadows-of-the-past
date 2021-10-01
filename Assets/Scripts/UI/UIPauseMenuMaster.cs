using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenuMaster : MonoBehaviour
{
    public static UIPauseMenuMaster Instance;

    public UIPokemonView pokemonViewerPrefab;
    public UIItemsViewer itemViewerPrefab;
    public AudioClip menuOpenSound;

    public TransitionBase menu;
    public GameObject opener;

    public Transform menuContainer;
    public Transform submenuContainer;

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
    public static UIPauseMenuMaster GetInstance() { return Instance; }

    public void HideOpener()
    {
        opener.SetActive(false);
    }
    public void ShowOpener()
    {
        opener.SetActive(true);
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
}

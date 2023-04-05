using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPokemonPCMaster : MonoBehaviour
{
    public static UIPokemonPCMaster Instance;

    public UIPokemonPC pokemonPCPrefab;

    public Transform pokemonPC;

    protected UIPokemonPC pokemonPCInstance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Show()
    {
        if (pokemonPCInstance)
        {
            Hide();
        }
        pokemonPCInstance = Instantiate(pokemonPCPrefab, pokemonPC).Load();
    }

    public void Hide()
    {
        if (pokemonPCInstance?.GetComponent<TransitionCanvasGroup>())
        {
            pokemonPCInstance?.GetComponent<TransitionCanvasGroup>().FadeIn();
        }
        else
        {
            Destroy(pokemonPCInstance.gameObject);
        }
    }
}

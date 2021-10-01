using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPokemonView : MonoBehaviour
{
    public UIBattleType battleTypePrefab;
    public UIBattleOptionsPokemon pokemonPrefab;

    public UIPokemonInfo pokemonSectionInfo;
    public UIPokemonMovesViewer pokemonSectionMoves;

    public Transform pokemonListContainer;
    public Transform typeList;
    public Transform pokemonSectionContainer;
    public TextMeshProUGUI pokemonName;
    public TextMeshProUGUI pokemonAbility;
    public Image pokemonSprite;
    public Image pokemonIcon;

    private PokemonAnimationController animator;

    private PokemonCaughtData currentPokemon;

    private void Start()
    {
        Load();
    }
    public void Load()
    {
        List<PokemonCaughtData> pokemonList = PartyMaster.GetInstance().GetParty();
        foreach (PokemonCaughtData pokemon in pokemonList)
        {
            UIBattleOptionsPokemon options = Instantiate(pokemonPrefab, pokemonListContainer).GetComponent<UIBattleOptionsPokemon>().Load(pokemon);
            options.onClick += (PokemonBattleData pkmn)=> LoadPokemon(pokemon);
        }
        PokemonCaughtData first = pokemonList[0];
        LoadPokemon(first);
       
    }

    public void LoadPokemon(PokemonCaughtData pkmn)
    {
        currentPokemon = pkmn;
        List<PokemonTypeId> types = pkmn.GetTypes();
        pokemonName.text = pkmn.GetName();
        pokemonAbility.text = AbilityMaster.GetInstance().GetAbility(pkmn.abilityId).GetName();
        foreach (Transform t in typeList)
            Destroy(t.gameObject);
        foreach (PokemonTypeId t in types)
            Instantiate(battleTypePrefab, typeList).GetComponent<UIBattleType>().Load(t);
        if (animator)
            Destroy(animator.gameObject);
        animator = Instantiate(pkmn.GetPokemonBaseData().battleAnimation);
        pokemonIcon.sprite = pkmn.GetPokemonBaseData().icon;
        ViewPokemonInfo();
    }
    public void ClearnSection()
    {
        foreach (Transform t in pokemonSectionContainer)
        {
            Destroy(t.gameObject);
        }
    }

    public void ViewPokemonInfo()
    {
        ClearnSection();
        Instantiate(pokemonSectionInfo, pokemonSectionContainer).GetComponent<UIPokemonInfo>().Load(currentPokemon);
    }
    public void ViewPokemonMoves()
    {
        ClearnSection();
        Instantiate(pokemonSectionMoves, pokemonSectionContainer).GetComponent<UIPokemonMovesViewer>().Load(currentPokemon);
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

    private void OnDestroy()
    {
        Destroy(animator.gameObject);
    }

    private void Update()
    {
        if (animator)
        {
            pokemonSprite.sprite = animator.sprite.sprite;
        }
    }
}

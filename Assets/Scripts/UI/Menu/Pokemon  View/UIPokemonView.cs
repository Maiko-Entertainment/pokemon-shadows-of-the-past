using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UIPokemonView : MonoBehaviour
{
    public UIBattleType battleTypePrefab;
    public UIItemOptionsPokemon pokemonPrefab;

    public UIPokemonInfo pokemonSectionInfo;
    public UIPokemonMovesViewer pokemonSectionMoves;

    public Transform pokemonListContainer;
    public Transform typeList;
    public Transform pokemonSectionContainer;
    public Transform sectionsListContainer;
    public TextMeshProUGUI pokemonName;
    public TextMeshProUGUI pokemonAbility;
    public Image pokemonSprite;
    public Image pokemonIcon;

    private PokemonAnimationController animator;

    private PokemonCaughtData currentPokemon;
    private int currentSectionIndex = 0;

    private void Start()
    {
        Load();
    }

    public void UpdatePokemonList(PokemonCaughtData newSelected)
    {
        currentPokemon = newSelected;
        List<PokemonCaughtData> pokemonList = PartyMaster.GetInstance().GetParty();
        foreach (Transform pokemon in pokemonListContainer)
        {
            Destroy(pokemon.gameObject);
        }
        foreach (PokemonCaughtData pokemon in pokemonList)
        {
            UIItemOptionsPokemon options = Instantiate(pokemonPrefab, pokemonListContainer).Load(pokemon);
            // options.onClick += (PokemonCaughtData p) => SelectPokemon();
            options.onHover += LoadPokemon;
            Button btn = options.GetComponent<Button>();
           
            // Sets first pokemon as selected
            if (currentPokemon == null && pokemonList.IndexOf(pokemon) == 0 || currentPokemon != null && currentPokemon == pokemon)
            {
                EventSystem eventSystem = EventSystem.current;
                eventSystem.SetSelectedGameObject(options.gameObject, new BaseEventData(eventSystem));
                currentPokemon = pokemon;
            }
        }
        UtilsMaster.LineSelectables(new List<Selectable>(pokemonListContainer.GetComponentsInChildren<Selectable>()));
    }
    public void ReturnToPokemonSelectionList()
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
    public void UpdatePokemonSelectedStatus(PokemonCaughtData pkmn)
    {
        foreach (Transform pokemon in pokemonListContainer)
        {
            pokemon.GetComponent<UIItemOptionsPokemon>().UpdateSelected(pkmn);
        }
    }

    public void Load()
    {
        UpdatePokemonList(PartyMaster.GetInstance().GetParty()[0]);
        LoadPokemon(currentPokemon);
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
        animator.transform.position = new Vector3(300000, 300000, 0);
        pokemonIcon.sprite = pkmn.GetPokemonBaseData().icon;
        UpdatePokemonSelectedStatus(pkmn);
        ViewCurrentSection();
    }

    public void ClearSection()
    {
        foreach (Transform t in pokemonSectionContainer)
        {
            Destroy(t.gameObject);
        }
    }

    public void HandleSectionChange()
    {
        foreach(Transform section in sectionsListContainer)
        {
            Transform selectedSection = sectionsListContainer.GetChild(currentSectionIndex);
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

    public void HandlePokemonMenuSection(CallbackContext context)
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
            ReturnToPokemonSelectionList();
        }
        else if (direction.x > 0)
        {
            currentSectionIndex = Mathf.Min(pokemonSectionContainer.childCount, currentSectionIndex + 1);
            ReturnToPokemonSelectionList();
        }
        if (previousIndex != currentSectionIndex)
        {
            ViewCurrentSection();
        }
    }
    public void ViewCurrentSection()
    {
        switch (currentSectionIndex)
        {
            case 0:
                ViewPokemonInfo();
                break;
            case 1:
                ViewPokemonMoves();
                break;
        }
    }

    public void ViewPokemonInfo()
    {
        ClearSection();
        Instantiate(pokemonSectionInfo, pokemonSectionContainer).GetComponent<UIPokemonInfo>().Load(currentPokemon);
        currentSectionIndex = 0;
        HandleSectionChange();
    }
    public void ViewPokemonMoves()
    {
        ClearSection();
        Instantiate(pokemonSectionMoves, pokemonSectionContainer).GetComponent<UIPokemonMovesViewer>().Load(currentPokemon);
        currentSectionIndex = 1;
        HandleSectionChange();
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

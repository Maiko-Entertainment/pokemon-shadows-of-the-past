using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UIPokedexView : MonoBehaviour
{
    public UIBattleType battleTypePrefab;
    public UIPokedexPokemonOption pokedexPokemonPrefab;
    public UIPokedexAbility pokedexAbilityPrefab;

    public UIPokedexSectionBasicData basicDataPrefab;

    public RectTransform pokemonListContainer;
    public RectTransform typeList;
    public RectTransform abilityList;
    public ScrollRect scrollRect;
    public Transform sectionContainer;
    public Transform sectionsListContainer;

    public TextMeshProUGUI pokemonName;
    public Image pokemonSprite;
    public Image pokemonIcon;

    public PokedexPokemonData selected;

    private PokemonAnimationController animator;
    private int sectionIndex = 0;

    void Start()
    {
        Load();
    }

    public void Load()
    {
        UpdatePokemonList();
    }

    public void UpdatePokemonList()
    {
        List<PokedexPokemonData> pokemonList = PokemonMaster.GetInstance().GetPokedexList();
        foreach (Transform pokemon in pokemonListContainer)
        {
            Destroy(pokemon.gameObject);
        }
        List<Selectable> selectables = new List<Selectable>();
        foreach (PokedexPokemonData pokemon in pokemonList)
        {
            UIPokedexPokemonOption options = Instantiate(pokedexPokemonPrefab, pokemonListContainer).Load(pokemon);
            // options.onClick += (PokemonCaughtData p) => SelectPokemon();
            options.onHover += LoadPokemon;
            selectables.Add(options.GetComponent<Selectable>());
            // Sets first pokemon as selected
            if (selected == null && pokemonList.IndexOf(pokemon) == 0 || selected != null && selected == pokemon)
            {
                UtilsMaster.SetSelected(selectables[0].gameObject);
                LoadPokemon(pokemon);
                selected = pokemon;
            }
        }
        UtilsMaster.LineSelectables(selectables);
    }
    public void UpdatePokemonSelectedStatus(PokedexPokemonData pkmn)
    {
        foreach (RectTransform pokemon in pokemonListContainer)
        {
            UIPokedexPokemonOption pokemonSelected = pokemon.GetComponent<UIPokedexPokemonOption>();
            pokemonSelected.UpdateSelected(pkmn);
            if (pokemonSelected.pokemon == pkmn)
                UtilsMaster.GetSnapToPositionToBringChildIntoView(scrollRect, pokemon);
        }
    }
    public void LoadPokemon(PokedexPokemonData data)
    {
        PokemonBaseData pkmn = data.pokemon;
        selected = data;

        List<PokemonTypeId> types = pkmn.types;
        pokemonName.text = pkmn.species;
        int totalPriorities = 0;
        foreach (Transform ability in abilityList)
        {
            Destroy(ability.gameObject);
        }
        foreach (PokemonBaseAbility ability in pkmn.abilities)
        {
            totalPriorities += ability.abilityPriority;
        }
        foreach (PokemonBaseAbility ability in pkmn.abilities)
        {
            AbilityData abilityData = AbilityMaster.GetInstance().GetAbility(ability.abilityId);
            string abilityName = "???";
            string percentage = "";
            if (data.caughtAmount > 0)
            {
                float priority = ability.abilityPriority;
                abilityName = abilityData.abilityName;
                percentage = (100 * (Mathf.Max(1f, priority) / Mathf.Max(1f, totalPriorities))).ToString("0") + "%";
            }
            Instantiate(pokedexAbilityPrefab, abilityList).GetComponent<UIPokedexAbility>().Load(abilityName, percentage);
        }
        foreach (RectTransform t in typeList)
            Destroy(t.gameObject);
        if (animator)
            Destroy(animator.gameObject);
        animator = Instantiate(pkmn.battleAnimation);
        animator.transform.position = new Vector3(300000, 300000, 0);
        pokemonIcon.sprite = pkmn.icon;
        if (data.seenAmount > 0)
        {
            pokemonSprite.color = Color.white;
            pokemonIcon.color = Color.white;
            pokemonName.text = pkmn.species;
            foreach (PokemonTypeId t in types)
                Instantiate(battleTypePrefab, typeList).GetComponent<UIBattleType>().Load(t);
        }
        else
        {
            pokemonSprite.color = Color.black;
            pokemonIcon.color = Color.black;
            pokemonName.text = "???";
        }
        UpdatePokemonSelectedStatus(data);
        LayoutRebuilder.ForceRebuildLayoutImmediate(typeList);
        ViewCurrentSection();
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
        UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
    }

    public void ViewCurrentSection()
    {
        switch (sectionIndex)
        {
            case 0:
                ViewBasicData();
                break;
        }
    }


    public void ViewBasicData()
    {
        ClearSection();
        Instantiate(basicDataPrefab, sectionContainer).Load(selected);
        sectionIndex = 0;
        HandleSectionChange();
    }
    public void HandleSectionChange()
    {
        foreach (Transform section in sectionsListContainer)
        {
            Transform selectedSection = sectionsListContainer.GetChild(sectionIndex);
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
    public void ClearSection()
    {
        foreach (Transform t in sectionContainer)
        {
            Destroy(t.gameObject);
        }
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

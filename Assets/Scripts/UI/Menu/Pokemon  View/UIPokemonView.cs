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

    public AudioClip onSelectSound;
    public AudioClip onSwapSound;

    public UIPokemonInfo pokemonSectionInfo;
    public UIPokemonStats pokemonSectionStats;
    public UIPokemonMovesViewer pokemonSectionMoves;
    public UIPokemonSettings pokemonSettings;

    public Transform pokemonListContainer;
    public Transform typeList;
    public Transform pokemonSectionContainer;
    public Transform sectionsListContainer;
    public TMP_InputField pokemonName;
    public TextMeshProUGUI pokemonAbility;
    public Image pokemonSprite;
    public Image pokemonIcon;
    public Image pokemonHeart;
    public Image pokemonHeartShattered;
    public GameObject swapHelp;
    public GameObject battlePickHelp;
    public UIPokemonGender genderIcon;

    private PokemonBattleAnimator animator;

    private PokemonCaughtData currentPokemon;
    private PokemonCaughtData swapingPokemon;
    private int currentSectionIndex = 0;
    private bool isInPreFaintPick = false;
    private bool isTyping = false;
    private bool closing = false;

    private void Start()
    {
        Load();
    }
    public void SetPrefaint(bool isInPreFaintPick)
    {
        this.isInPreFaintPick = isInPreFaintPick;
    }

    public void UpdatePokemonList(PokemonCaughtData newSelected)
    {
        currentPokemon = newSelected;
        List<PokemonCaughtData> pokemonList = PartyMaster.GetInstance().GetParty();
        foreach (Transform pokemon in pokemonListContainer)
        {
            Destroy(pokemon.gameObject);
        }
        List<Selectable> selectables = new List<Selectable>();
        int index = 0;
        foreach (PokemonCaughtData pokemon in pokemonList)
        {
            UIItemOptionsPokemon options = Instantiate(pokemonPrefab, pokemonListContainer).Load(pokemon);
            // options.onClick += (PokemonCaughtData p) => SelectPokemon();
            options.onHover += LoadPokemon;
            options.onHover += (PokemonCaughtData pkmn) => AudioMaster.GetInstance()?.PlaySfx(onSelectSound);
            Button btn = options.GetComponent<Button>();
            selectables.Add(options.GetComponent<Selectable>());
            // Sets first pokemon as selected
            if (currentPokemon == null && pokemonList.IndexOf(pokemon) == 0 || currentPokemon != null && currentPokemon == pokemon)
            {
                UtilsMaster.SetSelected(selectables[index].gameObject);
                currentPokemon = pokemon;
            }
            index++;
        }
        UtilsMaster.LineSelectables(selectables);
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
    public void HandleSwaping(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started && !isInPreFaintPick && !isTyping)
        {
            if (swapingPokemon != null)
            {
                Swap(currentPokemon);
            }
            else
            {
                swapingPokemon = currentPokemon;
                List<Selectable> selectables = new List<Selectable>();
                foreach (Transform pokemon in pokemonListContainer)
                {
                    UIItemOptionsPokemon option = pokemon.GetComponent<UIItemOptionsPokemon>();
                    if (option.pokemon != swapingPokemon)
                    {
                        selectables.Add(option.GetComponent<Button>());
                    }
                    if (currentPokemon == swapingPokemon)
                    {
                        currentPokemon = option.pokemon;
                    }
                    option.UpdateSwaping(swapingPokemon);
                    option.onClick += Swap;
                }
                UtilsMaster.LineSelectables(selectables);
                ReturnToPokemonSelectionList();
                AudioMaster.GetInstance()?.PlaySfx(onSelectSound);
            }
        }
    }
    public void Swap(PokemonCaughtData pokemon)
    {
        PartyMaster.GetInstance().SwapParty(swapingPokemon, currentPokemon);
        swapingPokemon = null;
        UpdatePokemonList(currentPokemon);

        AudioMaster.GetInstance()?.PlaySfx(onSwapSound);
        WorldMapMaster.GetInstance().GetPlayer().UpdatePokeFollower();
        UIPauseMenuMaster.GetInstance().UpdatePartyMiniPreview();
    }

    public void Load()
    {
        UpdatePokemonList(PartyMaster.GetInstance().GetParty()[0]);
        LoadPokemon(currentPokemon);
        bool isBattleActive = BattleMaster.GetInstance().IsBattleActive();
        if (isBattleActive)
        {
            battlePickHelp?.SetActive(true);
        }
        else
        {
            swapHelp?.SetActive(true);
        }
    }

    public void LoadPokemon(PokemonCaughtData pkmn)
    {
        currentPokemon = pkmn;
        List<PokemonTypeId> types = pkmn.GetTypes();
        if (pkmn.pokemonName == "")
        {
            pokemonName.transform.Find("Text Area").transform.Find("Placeholder").GetComponent<TMP_Text>().text = pkmn.GetName();
            pokemonName.text = "";
        }
        else
        {
            pokemonName.text = pkmn.GetName();
        }
        pokemonAbility.text = AbilityMaster.GetInstance().GetAbility(pkmn.abilityId).GetName();
        foreach (Transform t in typeList)
            Destroy(t.gameObject);
        foreach (PokemonTypeId t in types)
            Instantiate(battleTypePrefab, typeList).GetComponent<UIBattleType>().Load(t);
        if (animator)
            Destroy(animator.gameObject);
        animator = Instantiate(BattleAnimatorMaster.GetInstance().pokemonBattlerAnimator).Load(pkmn);
        animator.transform.position = new Vector3(300000, 300000, 0);
        pokemonIcon.sprite = pkmn.GetIcon();
        if (pkmn.isShadow)
        {
            pokemonHeart.gameObject.SetActive(false);
            pokemonHeartShattered.gameObject.SetActive(true);
        }
        else
        {
            pokemonHeart.gameObject.SetActive(true);
            pokemonHeartShattered.gameObject.SetActive(false);
            pokemonHeart.fillAmount = pkmn.GetFriendship() / 255f;
        }
        PokemonBaseData baseData = pkmn.GetPokemonBaseData();
        if (baseData.isGenderless)
        {
            genderIcon.gameObject.SetActive(false);
        }
        else
        {
            genderIcon.gameObject.SetActive(true);
            genderIcon.Load(pkmn.isMale);
        }
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
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started && !isTyping)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            int previousIndex = currentSectionIndex;
            if (IsCurrentMenuActive())
            {
                if (direction.x < 0)
                {
                    currentSectionIndex = Mathf.Max(0, currentSectionIndex - 1);
                    ReturnToPokemonSelectionList();
                }
                else if (direction.x > 0)
                {
                    currentSectionIndex = Mathf.Min(sectionsListContainer.childCount-1, currentSectionIndex + 1);
                    ReturnToPokemonSelectionList();
                }
                if (previousIndex != currentSectionIndex)
                {
                    ViewCurrentSection();
                }
                print("Change section from " + previousIndex + " to " + currentSectionIndex);
            }
        }
    }
    public void HandlePokemonPick(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started && !isTyping && !closing)
        {
            if (BattleMaster.GetInstance().IsBattleActive())
            {
                BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
                PokemonBattleData pbd = bm.GetTeamActivePokemon(BattleTeamId.Team1);
                PokemonBattleData currentActivePokemonBD = bm.GetPokemonFromCaughtData(currentPokemon);
                if (!currentPokemon.IsFainted() && pbd.battleId != currentActivePokemonBD.battleId)
                {
                    if (pbd.IsFainted())
                    {
                        bm.AddSwitchInPokemonEvent(bm.GetPokemonFromCaughtData(currentPokemon));
                    }
                    else
                    {
                        BattleMaster.GetInstance().GetCurrentBattle().HandleTurnInput(
                            new BattleTurnDesitionPokemonSwitch(
                                bm.GetPokemonFromCaughtData(currentPokemon),
                                BattleTeamId.Team1
                            ));
                    }
                    HandleClose();
                }
            }
        }
    }
    public void HandleClose(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started && !isInPreFaintPick && !isTyping)
        {
            HandleClose();
        }
    }

    public void HandleSubmit(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started && isTyping)
        {
            HandleClose();
        }
    }

    public void HandleClose()
    {
        closing = true;
        if (BattleMaster.GetInstance().IsBattleActive())
        {
            BattleAnimatorMaster.GetInstance().HidePokemonSelection(!isInPreFaintPick);
        }
        else
        {
            if (IsCurrentMenuActive())
                UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
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
                ViewPokemoStats();
                break;
            case 2:
                ViewPokemonMoves();
                break;
            case 3:
                ViewPokemonSettings();
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
    public void ViewPokemoStats()
    {
        ClearSection();
        Instantiate(pokemonSectionStats, pokemonSectionContainer).GetComponent<UIPokemonStats>().Load(currentPokemon);
        currentSectionIndex = 1;
        HandleSectionChange();
    }
    public void ViewPokemonMoves()
    {
        ClearSection();
        Instantiate(pokemonSectionMoves, pokemonSectionContainer).GetComponent<UIPokemonMovesViewer>().Load(currentPokemon);
        currentSectionIndex = 2;
        HandleSectionChange();
    }

    public void ViewPokemonSettings()
    {
        ClearSection();
        Instantiate(pokemonSettings, pokemonSectionContainer).Load(this);
        currentSectionIndex = 3;
        HandleSectionChange();
    }

    public bool IsCurrentMenuActive()
    {
        return BattleMaster.GetInstance().IsBattleActive() || UIPauseMenuMaster.GetInstance().GetCurrentMenu() == GetComponent<UIMenuPile>();
    }

    public void SetNameChanger()
    {
        UtilsMaster.SetSelected(pokemonName.gameObject);
        isTyping = true;
        pokemonName.transform.Find("Text Area").transform.Find("Placeholder").GetComponent<TMP_Text>().text = "";
    }
    public void SaveNameChanger(string newName)
    {
        // UtilsMaster.SetSelected(pokemonName.gameObject);
        isTyping = false;
        currentPokemon.SetName(newName);
        pokemonName.transform.Find("Text Area").transform.Find("Placeholder").GetComponent<TMP_Text>().text = currentPokemon.GetName();
        UIPauseMenuMaster.GetInstance().UpdatePartyMiniPreview();
        UpdatePokemonList(currentPokemon);
        ViewPokemonSettings();
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

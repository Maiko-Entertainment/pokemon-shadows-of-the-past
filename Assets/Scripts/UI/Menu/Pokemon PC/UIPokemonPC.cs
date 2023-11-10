using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UIPokemonPC : MonoBehaviour
{
    public Transform pcList;
    public Transform partyList;
    public int gridColumns = 4;

    public AudioClip onSelectSound;
    public AudioClip onSwapPickSound;
    public AudioClip onSwapSound;

    public UIPokemonPCElement pokemonPCPrefab;
    public UIItemOptionsPokemon pokemonPartyPrefab;
    public UIBattleType battleTypePrefab;
    public UIPokemonPCSwap pokemonSwapPrefab;
    public UIPokemonPCEmpty emptySlotPrefab;
    public Transform persistentInfo;

    public Image liveSprite;
    public Image pokemonIcon;
    public TextMeshProUGUI pokemonName;
    public TextMeshProUGUI pokemonLevel;
    public TextMeshProUGUI pokemonAbility;
    public Image pokemonHeart;
    public Image pokemonHeartShattered;

    public Transform typeList;
    public Transform pokemonSwapContainer;

    protected PokemonBattleAnimator animator;
    private PokemonCaughtData currentPokemon;

    private UIPokemonPCEmpty emptyPartySpace;
    private UIPokemonPCEmpty emptyBoxSpace;

    private bool isInBox = false;
    private PokemonCaughtData swapingPokemon;
    private UIPokemonPCSwap pokemonSwapInstance;

    public UIPokemonPC Load()
    {
        ClearPCPokemon();
        ClearPartyPokemon();
        List<PokemonCaughtData> pokemonPC = PartyMaster.Instance.GetPokemonInBox();
        List<PokemonCaughtData> pokemonParty = PartyMaster.Instance.GetParty();
        // Render Party
        foreach (PokemonCaughtData pokemon in pokemonParty)
        {
            UIItemOptionsPokemon item = Instantiate(pokemonPartyPrefab, partyList).Load(pokemon);
            item.onHover += HandlePokemonSelect;
            if (currentPokemon == null || currentPokemon == pokemon)
            {
                ViewPokemonInfo(pokemon);
                UtilsMaster.SetSelected(item.gameObject);
                isInBox = false;
            }
        }
        // Render PC
        foreach (PokemonCaughtData pokemon in pokemonPC)
        {
            UIPokemonPCElement item = Instantiate(pokemonPCPrefab, pcList).Load(pokemon);
            item.onHover += HandlePokemonSelect;
            if (currentPokemon == null || currentPokemon == pokemon)
            {
                ViewPokemonInfo(pokemon);
                UtilsMaster.SetSelected(item.gameObject);
                isInBox = true;
            }
        }
        LinePartyPokemon();
        GridBoxPokemon();
        return this;
    }

    public void ClearPCPokemon()
    {
        foreach(Transform poke in pcList)
        {
            Destroy(poke.gameObject);
        }
    }
    public void ClearPartyPokemon()
    {
        foreach (Transform poke in partyList)
        {
            Destroy(poke.gameObject);
        }
    }

    public async void LinePartyPokemon()
    {
        List<Selectable> selectables = new List<Selectable>();
        // Wait a frame for previus children to get destroyed
        await Task.Yield();
        foreach (Transform poke in partyList)
        {
            if (poke.GetComponent<Selectable>())
            {
                selectables.Add(poke.GetComponent<Selectable>());
            }
        }
        UtilsMaster.LineSelectables(selectables);
    }

    public async void GridBoxPokemon()
    {
        List<Selectable> selectables = new List<Selectable>();
        // Wait a frame for previus children to get destroyed
        await Task.Yield();
        foreach (Transform poke in pcList)
        {
            if (poke.GetComponent<Selectable>())
            {
                selectables.Add(poke.GetComponent<Selectable>());
            }
        }
        UtilsMaster.GridSelectables(selectables, gridColumns);
    }

    public void CreateEmptySpaces()
    {
        emptyBoxSpace = Instantiate(emptySlotPrefab, pcList);
        emptyBoxSpace.onHover += HandleEmptySelect;
        emptyPartySpace = Instantiate(emptySlotPrefab, partyList);
        emptyPartySpace.onHover += HandleEmptySelect;
        LinePartyPokemon();
        GridBoxPokemon();
    }

    public void HandleEmptySelect()
    {
        ViewPokemonInfo(null);
    }

    public void UpdateSwapingPosition(PokemonCaughtData poke)
    {
        if (pokemonSwapInstance)
        {
            if (isInBox)
            {
                if (poke == null)
                {
                    pokemonSwapInstance?.SetDestiny(emptyBoxSpace.transform);
                }
                else
                {
                    foreach (Transform p in pcList)
                    {
                        if (p.GetComponent<UIPokemonPCElement>() && p.GetComponent<UIPokemonPCElement>().pokemon == poke)
                        {
                            pokemonSwapInstance?.SetDestiny(p);
                            break;
                        }
                    }
                }
            }
            else
            {
                if (poke == null)
                {
                    pokemonSwapInstance?.SetDestiny(emptyPartySpace.transform);
                }
                else
                {
                    foreach (Transform p in partyList)
                    {
                        if (p.GetComponent<UIItemOptionsPokemon>() && p.GetComponent<UIItemOptionsPokemon>().pokemon == poke)
                        {
                            pokemonSwapInstance?.SetDestiny(p);
                            break;
                        }
                    }
                }
            }
        }
    }

    public void HandlePokemonSelect(PokemonCaughtData poke)
    {
        AudioMaster.Instance.PlaySfx(onSelectSound);
        ViewPokemonInfo(poke);
    }

    public void ViewPokemonInfo(PokemonCaughtData poke)
    {
        currentPokemon = poke;
        UpdateSwapingPosition(poke);
        if (animator)
            Destroy(animator.gameObject);
        if (currentPokemon == null)
        {
            persistentInfo?.gameObject.SetActive(false);
        }
        else
        {
            persistentInfo?.gameObject.SetActive(true);
            animator = Instantiate(BattleAnimatorMaster.GetInstance().pokemonBattlerAnimator).Load(poke);
            animator.transform.position = new Vector3(300000, 300000, 0);
            pokemonIcon.sprite = poke.GetIcon();
            pokemonName.text = currentPokemon.GetName();
            if (pokemonLevel)
            {
                pokemonLevel.text = "Lv. " + currentPokemon.GetLevel();
            }
            pokemonAbility.text = poke.ability.GetName();
            List<PokemonTypeId> types = poke.GetTypes();
            foreach (Transform t in typeList)
                Destroy(t.gameObject);
            foreach (PokemonTypeId t in types)
                Instantiate(battleTypePrefab, typeList).GetComponent<UIBattleType>().Load(t);
            if (poke.isShadow)
            {
                pokemonHeart.gameObject.SetActive(false);
                pokemonHeartShattered.gameObject.SetActive(true);
            }
            else
            {
                pokemonHeart.gameObject.SetActive(true);
                pokemonHeartShattered.gameObject.SetActive(false);
                pokemonHeart.fillAmount = poke.GetFriendship() / 255f;
            }
        }
    }

    public void SetPCToSelected()
    {
        StartCoroutine(UtilsMaster.SetSelectedNextFrame(pcList.GetChild(0).gameObject));
        isInBox = true;
        if (PartyMaster.GetInstance().pokemonBox.Count > 0)
            ViewPokemonInfo(pcList.GetChild(0).GetComponent<UIPokemonPCElement>().pokemon);
        else
            ViewPokemonInfo(null);
    }
    public void SetPartyToSelected()
    {
        UtilsMaster.SetSelected(partyList.GetChild(0).gameObject);
        isInBox = false;
        ViewPokemonInfo(partyList.GetChild(0).GetComponent<UIItemOptionsPokemon>().pokemon);
    }

    public void HandleClose(CallbackContext context)
    {
        bool isMenuOpen = UIPauseMenuMaster.GetInstance().GetCurrentMenu() == GetComponent<UIMenuPile>();
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started && isMenuOpen)
        {
            if (swapingPokemon == null)
            {
                HandleClose();
            }
            else
            {
                HandleCancelSwap();
            }
        }
    }
    public void HandleSectionChange(CallbackContext context)
    {
        bool isMenuOpen = UIPauseMenuMaster.GetInstance().GetCurrentMenu() == GetComponent<UIMenuPile>();
        if (!isMenuOpen) return;
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            // Press Right
            if (direction.x > 0 && !isInBox)
            {
                int childCount = pcList.childCount;
                if (childCount > 0)
                {
                    SetPCToSelected();
                }
            }
            else if (direction.x < 0 && isInBox)
            {
                int index = 0;
                foreach(Transform t in pcList)
                {
                    UIPokemonPCElement el = t.GetComponent<UIPokemonPCElement>();
                    if (el == null && currentPokemon == null || el.pokemon == currentPokemon)
                    {
                        if (index % gridColumns == 0)
                        {
                            SetPartyToSelected();
                        }
                    }
                    index++;
                }
            }
        }
    }
    public void HandleSwaping(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            if (swapingPokemon != null)
            {
                // We can't allow to leave the party empty!
                if (!IsPokemonInBox(swapingPokemon) &&
                    PartyMaster.GetInstance().GetParty().Count <= 1 &&
                    currentPokemon == null
                )
                {
                    return;
                }
                Swap(currentPokemon);
                AudioMaster.GetInstance()?.PlaySfx(onSwapSound);
                WorldMapMaster.GetInstance().GetPlayer().UpdatePokeFollower();
                UIPauseMenuMaster.GetInstance().UpdatePartyMiniPreview();
            }
            else
            {
                swapingPokemon = currentPokemon;
                pokemonSwapInstance = Instantiate(pokemonSwapPrefab, pokemonSwapContainer).Load(swapingPokemon);
                AudioMaster.GetInstance()?.PlaySfx(onSwapPickSound);
                foreach (Transform p in pcList)
                {
                    UIPokemonPCElement po = p.GetComponent<UIPokemonPCElement>();
                    po.SetCursorActive(false);
                    po.UpdateSwaping(currentPokemon);
                }
                UpdateSwapingPosition(currentPokemon);
                CreateEmptySpaces();
            }
        }
    }

    public void HandleRelease(CallbackContext context)
    {
        bool isMenuOpen = UIPauseMenuMaster.GetInstance().GetCurrentMenu() == GetComponent<UIMenuPile>();
        if (!isMenuOpen) return;
        bool hasEnoughToRelease = !(!IsPokemonInBox(currentPokemon) && PartyMaster.GetInstance().GetParty().Count <= 1);
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started && hasEnoughToRelease)
        {
            bool isPokemonShadow = currentPokemon.isShadow;
            if (!isPokemonShadow)
            {
                UIPauseMenuMaster.GetInstance().OpenConfirmMenu(
                    "Release " + currentPokemon.GetName() + "?",
                    "Once released, the pokemon will NEVER return.",
                    ConfirmPokemonRelease
                    );
            }
            else
            {
                UIPauseMenuMaster.GetInstance().OpenConfirmMenu(
                    "Err0R! Da%t4 c0rRu43tt3d!",
                    "Sy5tEm FAILURE.",
                    () => { }
                    );
            }
        }
    }
    protected void ConfirmPokemonRelease()
    {
        int boxIndex = 0;
        int boxCount = PartyMaster.GetInstance().pokemonBox.Count - 1; ;
        if (isInBox)
        {
            boxIndex = PartyMaster.GetInstance().pokemonBox.IndexOf(currentPokemon);
            PartyMaster.GetInstance().RemovePokemonInBox(currentPokemon);
        }
        else
        {
            PartyMaster.GetInstance().RemovePartyMember(currentPokemon);
        }
        currentPokemon = null;
        if (isInBox && boxCount > 0)
        {
            if (boxIndex < boxCount)
            {
                ViewPokemonInfo(PartyMaster.GetInstance().pokemonBox[boxIndex]);
            }
            else
            {
                ViewPokemonInfo(PartyMaster.GetInstance().pokemonBox[boxCount - 1]);
            }
        }
        HandleCancelSwap();
    }
    protected void QuickMoveToPC()
    {
        bool wasRemoved = PartyMaster.GetInstance().RemovePartyMember(currentPokemon);
        if (wasRemoved) // To avoid duplicating pokemon
            PartyMaster.GetInstance().AddPokemonToBox(currentPokemon);
        Load();
    }

    public void Swap(PokemonCaughtData current)
    {
        if (pokemonSwapInstance)
        {
            Destroy(pokemonSwapInstance.gameObject);
        }
        bool isSwapingPokemonFromBox = IsPokemonInBox(swapingPokemon);
        if (isInBox)
        {
            if (isSwapingPokemonFromBox)
            {
                if (current == null)
                {
                    bool didDelete = PartyMaster.Instance.RemovePokemonInBox(swapingPokemon);
                    if (didDelete) PartyMaster.Instance.AddPokemonToBox(swapingPokemon);
                }
                else
                {
                    PartyMaster.Instance.SwapPokemonInBox(current, swapingPokemon);
                }
            }
            else
            {
                if (current == null)
                {
                    PartyMaster.Instance.AddPokemonToBox(swapingPokemon);
                    PartyMaster.Instance.RemovePartyMember(swapingPokemon);
                }
                else
                {
                    int indexFromParty = PartyMaster.Instance.GetParty().IndexOf(swapingPokemon);
                    int indexFromBox = PartyMaster.Instance.GetPokemonInBox().IndexOf(current);
                    PartyMaster.Instance.AddPokemonToBox(swapingPokemon, indexFromBox);
                    PartyMaster.Instance.GetParty().Insert(indexFromParty, current);
                    PartyMaster.Instance.RemovePartyMember(swapingPokemon);
                    PartyMaster.Instance.RemovePokemonInBox(current);
                }
            }
        }
        else
        {
            if (isSwapingPokemonFromBox)
            {
                if (current == null)
                {
                    bool didDelete = PartyMaster.Instance.RemovePokemonInBox(swapingPokemon);
                    if (didDelete) PartyMaster.Instance.AddPartyMember(swapingPokemon);
                }
                else
                {
                    int indexFromParty = PartyMaster.Instance.GetParty().IndexOf(current);
                    int indexFromBox = PartyMaster.Instance.GetPokemonInBox().IndexOf(swapingPokemon);
                    PartyMaster.Instance.AddPokemonToBox(current, indexFromBox);
                    PartyMaster.Instance.GetParty().Insert(indexFromParty, swapingPokemon);
                    PartyMaster.Instance.RemovePartyMember(current);
                    PartyMaster.Instance.RemovePokemonInBox(swapingPokemon);
                }
            }
            else
            {
                if (current == null)
                {
                    bool didDelete = PartyMaster.Instance.RemovePartyMember(swapingPokemon);
                    if (didDelete) PartyMaster.Instance.AddPartyMember(swapingPokemon);
                }
                else
                {
                    PartyMaster.Instance.SwapPokemonInParty(current, swapingPokemon);
                }
            }
        }
        currentPokemon = swapingPokemon;
        swapingPokemon = null;
        Load();
    }
    public bool IsPokemonInBox(PokemonCaughtData pokemon)
    {
        foreach(Transform p in partyList)
        {
            UIItemOptionsPokemon options = p.GetComponent<UIItemOptionsPokemon>();
            if (options && options.pokemon == pokemon)
            {
                return false;
            }
        }
        return true;
    }

    public void HandleCancelSwap()
    {
        swapingPokemon = null;
        if (pokemonSwapInstance)
            Destroy(pokemonSwapInstance.gameObject);
        Load();
    }

    public void PrepareForSwap()
    {
        if (isInBox)
        {
            SetPartyToSelected();
            foreach (Transform pokemon in pcList)
            {
                UIPokemonPCElement pkmn = pokemon.GetComponent<UIPokemonPCElement>();
                pkmn.UpdateSwaping(swapingPokemon);
            }
        }
        else
        {
            SetPCToSelected();
        }
    }

    public void HandleClose()
    {
        UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
    }

    private void OnDestroy()
    {
        if (animator)
            Destroy(animator.gameObject);
    }

    private void Update()
    {
        if (animator)
        {
            liveSprite.sprite = animator.sprite.sprite;
        }
    }
}

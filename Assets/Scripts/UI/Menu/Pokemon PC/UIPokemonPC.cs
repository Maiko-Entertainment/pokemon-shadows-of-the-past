using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UIPokemonPC : MonoBehaviour
{
    public Transform pcList;
    public Transform partyList;
    public int gridRows = 4;

    public AudioClip onSelectSound;
    public AudioClip onSwapPickSound;
    public AudioClip onSwapSound;

    public UIPokemonPCElement pokemonPCPrefab;
    public UIItemOptionsPokemon pokemonPartyPrefab;
    public UIBattleType battleTypePrefab;
    public UIPokemonPCSwap pokemonSwapPrefab;

    public Image liveSprite;
    public Image pokemonIcon;
    public TextMeshProUGUI pokemonName;
    public TextMeshProUGUI pokemonLevel;
    public TextMeshProUGUI pokemonAbility;
    public Image pokemonHeart;
    public Image pokemonHeartShattered;

    public Transform typeList;
    public Transform pokemonSwapContainer;

    protected PokemonAnimationController animator;
    private PokemonCaughtData currentPokemon;

    private bool isInBox = false;
    private PokemonCaughtData swapingPokemon;
    private UIPokemonPCSwap pokemonSwapInstance;

    public UIPokemonPC Load()
    {
        ClearPCPokemon();
        ClearPartyPokemon();
        List<PokemonCaughtData> pokemonPC = PartyMaster.Instance.GetPokemonInBox();
        List<PokemonCaughtData> pokemonParty = PartyMaster.Instance.GetParty();
        List<Selectable> selectables = new List<Selectable>();
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
            selectables.Add(item.GetComponent<Selectable>());
        }
        UtilsMaster.LineSelectables(selectables);
        // Render PC
        selectables = new List<Selectable>();
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
            selectables.Add(item.GetComponent<Selectable>());
        }
        UtilsMaster.GridSelectables(selectables, gridRows);
        return this;
    }

    public void ClearPCPokemon()
    {
        foreach(Transform poke in partyList)
        {
            Destroy(poke.gameObject);
        }
    }
    public void ClearPartyPokemon()
    {
        foreach (Transform poke in pcList)
        {
            Destroy(poke.gameObject);
        }
    }

    public void UpdateSwapingPosition(PokemonCaughtData poke)
    {
        if (pokemonSwapInstance)
        {
            if (isInBox)
            {
                foreach (Transform p in pcList)
                {
                    if (p.GetComponent<UIPokemonPCElement>().pokemon == poke)
                    {
                        pokemonSwapInstance?.SetDestiny(p);
                        break;
                    }
                }
            }
            else
            {
                foreach (Transform p in partyList)
                {
                    if (p.GetComponent<UIItemOptionsPokemon>().pokemon == poke)
                    {
                        pokemonSwapInstance?.SetDestiny(p);
                        break;
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
        animator = Instantiate(poke.GetPokemonBaseData().GetAnimatorController());
        animator.transform.position = new Vector3(300000, 300000, 0);
        pokemonIcon.sprite = poke.GetPokemonBaseData().icon;
        pokemonName.text = currentPokemon.GetName();
        if (pokemonLevel)
        {
            pokemonLevel.text = "Lv. " + currentPokemon.GetLevel();
        }
        pokemonAbility.text = AbilityMaster.GetInstance().GetAbility(poke.abilityId).GetName();
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

    public void SetPCToSelected()
    {
        StartCoroutine(UtilsMaster.SetSelectedNextFrame(pcList.GetChild(0).gameObject));
        isInBox = true;
        ViewPokemonInfo(pcList.GetChild(0).GetComponent<UIPokemonPCElement>().pokemon);
    }
    public void SetPartyToSelected()
    {
        UtilsMaster.SetSelected(partyList.GetChild(0).gameObject);
        isInBox = false;
        ViewPokemonInfo(partyList.GetChild(0).GetComponent<UIItemOptionsPokemon>().pokemon);
    }

    public void HandleClose(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
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
                    if (el.pokemon == currentPokemon)
                    {
                        if (index % 4 == 0)
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
            }
        }
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
                PartyMaster.Instance.SwapPokemonInBox(current, swapingPokemon);
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
        else
        {
            if (isSwapingPokemonFromBox)
            {
                int indexFromParty = PartyMaster.Instance.GetParty().IndexOf(current);
                int indexFromBox = PartyMaster.Instance.GetPokemonInBox().IndexOf(swapingPokemon);
                PartyMaster.Instance.AddPokemonToBox(current, indexFromBox);
                PartyMaster.Instance.GetParty().Insert(indexFromParty, swapingPokemon);
                PartyMaster.Instance.RemovePartyMember(current);
                PartyMaster.Instance.RemovePokemonInBox(swapingPokemon);
            }
            else
            {
                PartyMaster.Instance.SwapPokemonInParty(current, swapingPokemon);
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
            if (p.GetComponent<UIItemOptionsPokemon>().pokemon == pokemon)
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

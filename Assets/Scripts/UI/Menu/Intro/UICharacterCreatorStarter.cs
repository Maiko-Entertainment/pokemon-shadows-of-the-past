using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UICharacterCreatorStarter : MonoBehaviour
{
    public UIMenuPile nextMenu;
    public Transform starterList;
    public Image starterPreviewImage;
    public TMP_InputField starterNickname;
    public TextMeshProUGUI starterName;
    public TextMeshProUGUI starterDescription;
    public Transform typesList;
    public ScrollRect scrollRect;


    public UIStarterPicker starterPickerPrefab;
    public UIBattleType typePrefab;
    public UIPokemonGender genderViewer;
    public AudioClip flipGenderSound;

    public List<PokemonBaseData> starterPokemon = new List<PokemonBaseData>();

    private PokemonAnimationController animatorCheat;
    private PokemonBaseData pickedStarter;
    private bool isStarterMale = true;
    private void Start()
    {
        Load();
    }

    public void Load()
    {
        foreach (Transform t in starterList)
        {
            Destroy(t.gameObject);
        }
        int index = 0;
        foreach (PokemonBaseData p in starterPokemon)
        {
            UIStarterPicker starter = Instantiate(starterPickerPrefab, starterList).Load(p);
            starter.onSelect += (starterData) => PreviewStarter(starterData, starter.GetComponent<RectTransform>());
            if (index == 0)
            {
                 PreviewStarter(starterPokemon[0], starter.GetComponent<RectTransform>());
            }
            index++;
        }
        genderViewer.Load(isStarterMale);
    }
    public void PreviewStarter(PokemonBaseData pokemon, RectTransform button)
    {
        StartCoroutine(ViewStarterNextFrame(pokemon, button));
    }
    IEnumerator ViewStarterNextFrame(PokemonBaseData pokemon, RectTransform button)
    {
        pickedStarter = pokemon;
        yield return new WaitForEndOfFrame();
        UtilsMaster.SetSelected(button.gameObject);
        starterName.text = pokemon.species;
        starterDescription.text = pokemon.GetPokedexEntry();
        if (animatorCheat)
            Destroy(animatorCheat.gameObject);
        animatorCheat = Instantiate(pokemon.battleAnimation);
        animatorCheat.transform.position = new Vector3(3000, 3000);
        foreach (Transform t in typesList)
        {
            Destroy(t.gameObject);
        }
        foreach (PokemonTypeId t in pokemon.types)
        {
            Instantiate(typePrefab, typesList).Load(t);
        }
        UtilsMaster.GetSnapToPositionToBringChildIntoView(scrollRect, button);
    }

    public void FlipGender()
    {
        AudioMaster.GetInstance().PlaySfx(flipGenderSound);
        isStarterMale = !isStarterMale;
        if (genderViewer)
        {
            genderViewer.Load(isStarterMale);
        }
    }

    public void HandleSubmit()
    {
        SaveElement characterPicked = SaveMaster.Instance.GetSaveElement(SaveElementId.starterPickedId);
        characterPicked.SetValue((float)pickedStarter.pokemonId);

        PokemonCaughtData starter = new PokemonCaughtData();
        starter.pokemonBase = pickedStarter;
        starter.level = 5;
        starter.isMale = isStarterMale;
        List<PokemonNatureId> natures = new List<PokemonNatureId>() { PokemonNatureId.careful, PokemonNatureId.cunning, PokemonNatureId.reserved, PokemonNatureId.restless, PokemonNatureId.ruthless };
        starter.natureId = natures[Random.Range(0, 4)];
        starter.CheckForLearnedMoves(starter.level);
        starter.abilityId = pickedStarter.GetRandomAbility();
        starter.friendship = pickedStarter.baseFriendship;
        PartyMaster.GetInstance().AddPartyMember(starter);
        SaveElement se = SaveMaster.Instance.GetSaveElement(SaveElementId.startedTypePicked);
        SaveElementNumber sen = (SaveElementNumber)se;
        if (starter.GetTypes().Contains(PokemonTypeId.Fire))
        {
            sen.SetValue(3f);
        }
        else if (starter.GetTypes().Contains(PokemonTypeId.Water))
        {
            sen.SetValue(2f);
        }
        else
        {
            sen.SetValue(1f);
        }
        PokemonMaster.GetInstance().CaughtPokemon(starter.GetPokemonBaseData().pokemonId);
        UIPauseMenuMaster.GetInstance()?.OpenMenu(nextMenu, true);
    }

    public void HandleSubmit(CallbackContext context)
    {
        bool interactable = GetComponent<CanvasGroup>().interactable;
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started && interactable)
        {
            HandleSubmit();
        }
    }
    public void HandleSwapGender(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            FlipGender();
        }
    }

    public void HandleGoBack(CallbackContext context)
    {
        bool interactable = GetComponent<CanvasGroup>().interactable;
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started && interactable)
        {
            UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
        }
    }
    private void Update()
    {
        if (animatorCheat)
        {
            starterPreviewImage.sprite = animatorCheat.sprite.sprite;
        }
    }
}

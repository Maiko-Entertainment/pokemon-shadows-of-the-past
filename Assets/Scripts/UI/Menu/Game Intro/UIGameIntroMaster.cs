using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameIntroMaster : MonoBehaviour
{
    public static UIGameIntroMaster Instance;

    public TransitionBase characterData;
    public TransitionBase starterSelector;

    public TMP_InputField nameInput;
    public Transform characterList;
    public Button submitcharacterData;
    // Starter Picker
    public Transform starterList;
    public Image starterPreviewImage;
    public TMP_InputField starterNickname;
    public TextMeshProUGUI starterName;
    public TextMeshProUGUI starterDescription;
    public Transform typesList;
    public UIGenderButton genderButton;
    public Button submitStarter;

    public UICharacterPicker pickerPrefab;
    public UIStarterPicker starterPickerPrefab;
    public UIBattleType typePrefab;
    public List<PokemonBaseData> starterPokemon = new List<PokemonBaseData>();

    public AudioClip openMenuSound;
    public AudioClip submitSound;

    private PokemonAnimationController animatorCheat;
    private PokemonBaseData pickedStarter;
    private bool isStarterMale = true;

    private void Start()
    {
        // OpenPlayerDataPanel();
        SubmitPlayerData();
    }

    public static UIGameIntroMaster GetInstance()
    {
        return Instance;
    }
    public void OnChangeName(string name)
    {
        submitcharacterData.interactable = name != "";
    }

    public void OpenPlayerDataPanel()
    {
        starterSelector?.FadeOut();
        AudioMaster.GetInstance().PlaySfx(openMenuSound);
        characterData?.FadeIn();
        foreach (Transform t in characterList)
        {
            Destroy(t.gameObject);
        }
        int playerAmounts = WorldMapMaster.GetInstance().playerPrefabs.Count;
        for (int i = 0; i < playerAmounts; i++)
        {
            Instantiate(pickerPrefab, characterList).Load(i);
        }
    }

    public void SubmitPlayerData()
    {
        SaveElement playerName = SaveMaster.Instance.GetSaveElement(SaveElementId.playerName);
        playerName.SetValue(nameInput.text);
        characterData?.FadeOut();
        AudioMaster.GetInstance().PlaySfx(submitSound);
        starterSelector?.FadeIn();
        foreach (Transform t in starterList)
        {
            Destroy(t.gameObject);
        }
        foreach (PokemonBaseData p in starterPokemon)
        {
            UIStarterPicker starter = Instantiate(starterPickerPrefab, starterList).Load(p);
            starter.OnClick += PreviewStarter;
        }
        genderButton.Load(isStarterMale);
        PreviewStarter(starterPokemon[0]);
    }

    public void PreviewStarter(PokemonBaseData pokemon)
    {
        pickedStarter = pokemon;
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
    }

    public void FlipGender()
    {
        AudioMaster.GetInstance().PlaySfx(openMenuSound);
        isStarterMale = !isStarterMale;
        genderButton.Load(isStarterMale);
    }

    public void GoToNextScene()
    {
        PokemonCaughtData starter = new PokemonCaughtData();
        starter.pokemonBase = pickedStarter;
        starter.level = 5;
        starter.pokemonName = starterNickname.text;
        starter.isMale = isStarterMale;
        List<PokemonNatureId> natures = new List<PokemonNatureId>() { PokemonNatureId.careful, PokemonNatureId.cunning, PokemonNatureId.reserved, PokemonNatureId.restless, PokemonNatureId.ruthless };
        starter.natureId = natures[Random.Range(0,4)];
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
        else if(starter.GetTypes().Contains(PokemonTypeId.Water))
        {
            sen.SetValue(2f);
        }
        else
        {
            sen.SetValue(1f);
        }
        starterSelector?.FadeOut();
        AudioMaster.GetInstance().PlaySfx(submitSound);
        InteractionsMaster.GetInstance().ExecuteNext();
    }

    private void Update()
    {
        if (animatorCheat)
        {
            starterPreviewImage.sprite = animatorCheat.sprite.sprite;
        }
    }
}

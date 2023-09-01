using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMaster : MonoBehaviour
{
    public static ResourceMaster Instance;

    public string pokemonBattlerPath = "Sprites/Pokemon/{pokemon}/{pokemon}";
    public string battlerFrontIndicator = "";
    public string battlerBackIndicator = "_b";
    public string pokemonWorldPath = "Sprites/Pokemon/{pokemon}/{pokemon}_world";
    public string pokemonIconPath = "Sprites/Pokemon/{pokemon}/{pokemon}_icon";
    public string pokemonCryPath = "Audio/Sounds/Pokemon/{pokemon}";
    public string shinyIndicator = "_s";
    public string femaleIndicator = "_fe";
    public string pokemonDataPath = "ScriptableObjects/Pokemon/BaseData";
    public string pokemonAbilityPath = "ScriptableObjects/Pokemon/Abilities";
    public string pokemonMovesPath = "ScriptableObjects/Pokemon/Moves";
    public string itemsCategoryPath = "ScriptableObjects/Items/{category}";
    public string saveElementsPath = "ScriptableObjects/SaveElements";

    private void Awake()
    {
        Instance = this;
    }

    public string GetBaseBattlerPath(PokemonBaseData pokemon)
    {
        string parsed = pokemonBattlerPath.Replace("{pokemon}", pokemon.name);
        return parsed;
    }
    public string GetBaseWorldPath(PokemonBaseData pokemon)
    {
        string parsed = pokemonWorldPath.Replace("{pokemon}", pokemon.name);
        return parsed;
    }
    public string GetBaseIconPath(PokemonBaseData pokemon)
    {
        string parsed = pokemonIconPath.Replace("{pokemon}", pokemon.name);
        return parsed;
    }
    public string GetBasePokemonCryPath(PokemonBaseData pokemon)
    {
        string parsed = pokemonCryPath.Replace("{pokemon}", pokemon.name);
        return parsed;
    }
    public string GetPokemonDataPath()
    {
        return pokemonDataPath;
    }
    public string GetAbilityDataPath()
    {
        return pokemonAbilityPath;
    }
    public string GetMovesDataPath()
    {
        return pokemonMovesPath;
    }

    public string GetItemCategoryPath()
    {
        return itemsCategoryPath;
    }
    public string GetSaveElementsPath()
    {
        return saveElementsPath;
    }
}

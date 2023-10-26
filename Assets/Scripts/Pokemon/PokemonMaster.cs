using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonMaster : MonoBehaviour
{
    public static PokemonMaster Instance;
    public List<PokemonBaseData> pokedex = new List<PokemonBaseData>();
    public List<PokemonBaseData> otherPokemon = new List<PokemonBaseData>();

    public bool evolveFirstPartyPokemon = false;
    public PokemonBaseData forceEvolveTo;

    protected Dictionary<string, PokedexPokemonData> pokedexData = new Dictionary<string, PokedexPokemonData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InstantiateDatabase();
        }
        else
        {
            Destroy(this);
        }
    }

    public void InstantiateDatabase()
    {
        PokemonBaseData[] baseDatas = Resources.LoadAll<PokemonBaseData>(ResourceMaster.Instance.GetPokemonDataPath());
        foreach (PokemonBaseData pokemonData in baseDatas)
        {
            pokedexData.Add(pokemonData.pokemonId, new PokedexPokemonData(pokemonData));
        }
    }

    private void Start()
    {
        if (evolveFirstPartyPokemon)
        {
            StartCoroutine(EvolveAfter(1f));
        }
    }

    public void Load(SaveFile saveFile)
    {
        foreach(PersistedPokedexPokemonData pData in saveFile.persistedPokedexPokemonData)
        {
            if (pokedexData.ContainsKey(pData.GetId()))
            {
                pokedexData[pData.GetId()].seenAmount = pData.seenAmount;
                pokedexData[pData.GetId()].caughtAmount = pData.caughtAmount;
            }
        }
    }

    public void HandleSave()
    {
        List<PersistedPokedexPokemonData> persistedPokedexPokemon = new List<PersistedPokedexPokemonData>();
        foreach (PokedexPokemonData pd in pokedexData.Values)
        {
            persistedPokedexPokemon.Add(pd.GetSave());
        }
        SaveMaster.Instance.activeSaveFile.persistedPokedexPokemonData = persistedPokedexPokemon;
    }

    IEnumerator EvolveAfter(float after)
    {
        yield return new WaitForSeconds(after);
        InteractionsMaster.GetInstance().AddEvent(new InteractionEventStartEvolution(PartyMaster.GetInstance().GetParty()[0], forceEvolveTo));
    }

    public static PokemonMaster GetInstance()
    {
        return Instance;
    }

    public List<PokedexPokemonData> GetPokedexList()
    {
        List<PokedexPokemonData> list = new List<PokedexPokemonData>();
        foreach (PokemonBaseData pkmn in pokedex)
        {
            list.Add(GetPokemonPokedexData(pkmn.pokemonId));
        }
        return list;
    }

    public List<PokedexPokemonData> GetSpecialPokemon()
    {
        List<PokedexPokemonData> list = new List<PokedexPokemonData>();
        foreach (PokemonBaseData pkmn in otherPokemon)
        {
            list.Add(GetPokemonPokedexData(pkmn.pokemonId));
        }
        return list;
    }

    public PokedexPokemonData GetPokemonPokedexData(PokemonBaseId pokemonId)
    {
        return GetPokemonPokedexData(pokemonId.ToString());
    }
    public PokedexPokemonData GetPokemonPokedexData(string pokemonId)
    {
        if (pokedexData.ContainsKey(pokemonId))
        {
            return pokedexData[pokemonId];
        }
        return null;
    }

    public void SeePokemon(string pokemonId)
    {
        if (pokedexData.ContainsKey(pokemonId))
        {
            pokedexData[pokemonId].seenAmount++;
        }
    }
    public void CaughtPokemon(string pokemonId)
    {
        if (pokedexData.ContainsKey(pokemonId))
        {
            pokedexData[pokemonId].caughtAmount++;
        }
    }

    public PokemonBaseData GetPokemonData(PokemonBaseId id)
    {
        return GetPokemonData(id.ToString());
    }

    public PokemonBaseData GetPokemonData(string id)
    {
        PokedexPokemonData data = GetPokemonPokedexData(id);
        if (data != null && data.pokemon != null)
            return data.pokemon;
        return null;
    }

    public void EvolvePokemon(PokemonCaughtData pokemon, PokemonBaseData evolution)
    {
        PokemonCaughtData original = pokemon.Copy();

        AbilityId originalAbility = pokemon.abilityId;
        bool foundSameAbilty = false;
        foreach(PokemonBaseAbility ability in evolution.abilities)
        {
            if (originalAbility == ability.abilityId)
            {
                foundSameAbilty = true;
            }
        }
        if (!foundSameAbilty)
        {
            originalAbility = evolution.GetRandomAbility();
        }
        pokemon.pokemonBase = evolution;
        pokemon.abilityId = originalAbility;
        List<PokemonMoveLearn> learnedMoves = pokemon.CheckForLearnedMoves(pokemon.GetLevel());
        UIEvolutionMaster.GetInstance().InitiateEvolution(original, evolution, learnedMoves);
        SeePokemon(evolution.pokemonId);
        CaughtPokemon(evolution.pokemonId);
    }

    public bool CheckForEvolution(PokemonCaughtData pokemon)
    {
        List<PokemonBaseEvolution> evolutions = pokemon.GetPokemonBaseData().evolutions;
        foreach(PokemonBaseEvolution evo in evolutions)
        {
            if (evo.CanEvolve(pokemon))
            {
                PokemonBaseData evolution = evo.pokemon;
                InteractionsMaster.GetInstance().AddEvent(new InteractionEventStartEvolution(pokemon, evolution));
                return true;
            }
        }
        return false;
    }
}

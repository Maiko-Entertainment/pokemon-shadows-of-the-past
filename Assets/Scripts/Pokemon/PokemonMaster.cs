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

    protected Dictionary<PokemonBaseId, PokedexPokemonData> pokedexData = new Dictionary<PokemonBaseId, PokedexPokemonData>();

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
        foreach (PokemonBaseData pokemonData in pokedex)
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
            if (pokedexData.ContainsKey(pData.pokemonId))
            {
                pokedexData[pData.pokemonId].seenAmount = pData.seenAmount;
                pokedexData[pData.pokemonId].caughtAmount = pData.caughtAmount;
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
        return pokedexData.Values.ToList();
    }
    public PokedexPokemonData GetPokemonPokedexData(PokemonBaseId pokemonId)
    {
        foreach(PokedexPokemonData pokemon in pokedexData.Values.ToList())
        {
            if (pokemonId == pokemon.pokemon.pokemonId)
            {
                return pokemon;
            }
        }
        return null;
    }

    public void SeePokemon(PokemonBaseId pokemonId)
    {
        if (pokedexData.ContainsKey(pokemonId))
        {
            pokedexData[pokemonId].seenAmount++;
        }
    }
    public void CaughtPokemon(PokemonBaseId pokemonId)
    {
        if (pokedexData.ContainsKey(pokemonId))
        {
            pokedexData[pokemonId].caughtAmount++;
        }
    }

    public PokemonBaseData GetPokemonData(PokemonBaseId id)
    {
        foreach (PokemonBaseData pkmn in pokedex)
        {
            if (pkmn.pokemonId == id)
                return pkmn;
        }
        foreach (PokemonBaseData pkmn in otherPokemon)
        {
            if (pkmn.pokemonId == id)
                return pkmn;
        }
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
    }

    public bool CheckForEvolution(PokemonCaughtData pokemon)
    {
        List<PokemonBaseEvolution> evolutions = pokemon.GetPokemonBaseData().evolutions;
        foreach(PokemonBaseEvolution evo in evolutions)
        {
            if (evo.CanEvolve(pokemon))
            {
                PokemonBaseData evolution = GetPokemonData(evo.pokemonId);
                InteractionsMaster.GetInstance().AddEvent(new InteractionEventStartEvolution(pokemon, evolution));
                return true;
            }
        }
        return false;
    }
}

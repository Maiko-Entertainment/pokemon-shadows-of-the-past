using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonMaster : MonoBehaviour
{
    public static PokemonMaster Instance;
    public List<PokemonBaseData> pokedex = new List<PokemonBaseData>();
    public List<PokemonBaseData> otherPokemon = new List<PokemonBaseData>();

    public bool evolveFirstPartyPokemon = false;
    public PokemonBaseData forceEvolveTo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        if (evolveFirstPartyPokemon)
        {
            StartCoroutine(EvolveAfter(1f));
        }
    }

    IEnumerator EvolveAfter(float after)
    {
        yield return new WaitForSeconds(after);
        EvolvePokemon(PartyMaster.GetInstance().GetParty()[0], forceEvolveTo);
    }

    public static PokemonMaster GetInstance()
    {
        return Instance;
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
        UIEvolutionMaster.GetInstance().InitiateEvolution(pokemon, evolution);
    }
}

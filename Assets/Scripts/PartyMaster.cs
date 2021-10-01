using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMaster : MonoBehaviour
{
    public static PartyMaster Instance;
    public static int maxParty = 6;

    public List<PokemonCaughtData> party = new List<PokemonCaughtData>();
    public List<PokemonCaughtData> pokemonBox = new List<PokemonCaughtData>();

    public bool loadDefaultParty = false;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public static PartyMaster GetInstance()
    {
        return Instance;
    }

    public void Load(SaveFile save)
    {
        if (!loadDefaultParty)
        {
            party = new List<PokemonCaughtData>();
            foreach (PersistedPokemon pp in save.persistedParty)
            {
                party.Add(new PokemonCaughtData(pp));
            }
        }
    }

    public void HandleSave()
    {
        List<PersistedPokemon> persistedPokemon = new List<PersistedPokemon>();
        foreach(PokemonCaughtData pokemon in party)
        {
            persistedPokemon.Add(pokemon.GetSave());
        }
        SaveMaster.Instance.activeSaveFile.persistedParty = persistedPokemon;
    }

    public List<PokemonCaughtData> GetParty()
    {
        return party;
    }

    public void AddPartyMember(PokemonCaughtData newPokemon)
    {
        if (CanAddPartyMember())
        {
            party.Add(newPokemon);
        }
    }

    public bool CanAddPartyMember()
    {
        return party.Count < maxParty;
    }
}

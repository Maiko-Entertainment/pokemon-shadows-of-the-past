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
            pokemonBox = new List<PokemonCaughtData>();
            foreach (PersistedPokemon pp in save.persistedBox)
            {
                pokemonBox.Add(new PokemonCaughtData(pp));
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
        List<PersistedPokemon> persistedBox = new List<PersistedPokemon>();
        foreach (PokemonCaughtData pokemon in pokemonBox)
        {
            persistedBox.Add(pokemon.GetSave());
        }
        SaveMaster.Instance.activeSaveFile.persistedParty = persistedPokemon;
        SaveMaster.Instance.activeSaveFile.persistedBox = persistedBox;
    }

    public List<PokemonCaughtData> GetParty()
    {
        return party;
    }

    public void SwapParty(PokemonCaughtData swaping, PokemonCaughtData swaper)
    {
        int swapingIndex = party.IndexOf(swaping);
        int swapedIndex = party.IndexOf(swaper);
        PokemonCaughtData pokemonAux = party[swapingIndex];
        party[swapingIndex] = party[swapedIndex];
        party[swapedIndex] = pokemonAux;
    }

    // Returns true if the pokemon was added to the party and false if it was sent to the box
    public bool AddPartyMember(PokemonCaughtData newPokemon)
    {
        if (CanAddPartyMember())
        {
            party.Add(newPokemon);
            UIPauseMenuMaster.GetInstance().UpdatePartyMiniPreview();
            return true;
        }
        else
        {
            pokemonBox.Add(newPokemon);
            return false;
        }
    }

    public bool CanAddPartyMember()
    {
        return party.Count < maxParty;
    }

    public PokemonCaughtData GetFirstAvailablePokemon()
    {
        foreach(PokemonCaughtData pkmn in party)
        {
            if (!pkmn.IsFainted())
                return pkmn;
        }
        return null;
    }

    public void HealAll()
    {
        foreach(PokemonCaughtData pkmn in party)
        {
            FullyHeal(pkmn);
        }
        UIPauseMenuMaster.GetInstance().UpdatePartyMiniPreview();
    }
    public void FullyHeal(int partyIndex)
    {
        PokemonCaughtData pkmn = party[partyIndex];
        FullyHeal(pkmn);
    }
    public void FullyHeal(PokemonCaughtData pkmn)
    {
        pkmn.ChangeHealth(9999);
    }
}

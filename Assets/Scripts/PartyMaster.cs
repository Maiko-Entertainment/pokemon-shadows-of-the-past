using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMaster : MonoBehaviour
{
    public static PartyMaster Instance;
    public static int maxParty = 6;

    public List<PokemonCaughtData> party = new List<PokemonCaughtData>();

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

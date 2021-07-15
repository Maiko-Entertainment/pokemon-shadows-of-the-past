using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonCaughtMaster : MonoBehaviour
{
    public static PokemonCaughtMaster Instance;
    public List<PokemonCaughtData> party;

    public List<PokemonCaughtData> box;
    public void Awake()
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
    public static PokemonCaughtMaster GetInstance() { return Instance; }

    public List<PokemonCaughtData> GetPlayerParty()
    {
        return party;
    }

}

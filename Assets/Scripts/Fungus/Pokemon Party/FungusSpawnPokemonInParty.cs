using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Game",
    "Spawn Party Pokemon",
    "Spawn a pokemon in the party in the world."
)]
public class FungusSpawnPokemonInParty : Command
{
    public Transform spawnPoint;
    public int partyIndex = 0;
    public MoveBrainDirection faceDirection = MoveBrainDirection.Bottom;
    public bool addToAgents = true;
    public string overrideAgentName = "";

    public override void OnEnter()
    {
        List<PokemonCaughtData> party = PartyMaster.GetInstance().GetParty();
        if (party.Count > partyIndex)
        {
            PokemonCaughtData pkmn = party[partyIndex];
            WorldInteractableWorldBrainPokemon agent = Instantiate(WorldMapMaster.GetInstance().pokeFollowerPrefab, spawnPoint).Load(pkmn);
            agent.followMode = false;
            MoveBrainDirectionData dir = new MoveBrainDirectionData(faceDirection, true);
            agent.AddDirection(dir);
            if (addToAgents)
            {
                if (overrideAgentName != "")
                {
                    agent.moveIdentifier = overrideAgentName;
                }
                CutsceneMaster.GetInstance().GetCurrentCutscene().cutsceneAgents.Add(agent);
            }
        }
        Continue();
    }
}

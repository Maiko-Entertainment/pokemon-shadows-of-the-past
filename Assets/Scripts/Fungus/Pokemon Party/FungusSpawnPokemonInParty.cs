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
            if (pkmn.GetPokemonBaseData().GetOverWorldPrefab())
            {
                WorldInteractableBrainFollower agent = Instantiate(pkmn.GetPokemonBaseData().GetOverWorldPrefab(), spawnPoint);
                agent.followMode = false;
                MoveBrainDirectionData dir = new MoveBrainDirectionData();
                dir.direction = faceDirection;
                dir.justTurn = true;
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
        }
        Continue();
    }
}

using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public Flowchart flowchart;
    public string blockName = "Start";

    public List<WorldInteractable> cutsceneAgents = new List<WorldInteractable>();

    protected List<WorldInteractable> spawnedAgents = new List<WorldInteractable>();
    protected bool wasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !wasTriggered)
        {
            AddEvent();
        }
    }

    public void AddEvent()
    {
        wasTriggered = true;
        InteractionsMaster.GetInstance().AddEvent(new InteractionEventCutscene(this));
    }

    public void Initiate()
    {
        CutsceneMaster.GetInstance().LoadCutscene(this);
        foreach(WorldInteractable ca in cutsceneAgents)
        {
            WorldInteractable agentInstance = Instantiate(ca, transform);
            spawnedAgents.Add(agentInstance);
        }
        flowchart.ExecuteBlock(blockName);
    }

    public WorldInteractable GetAgent(string agentId)
    {
        foreach(WorldInteractable spawnedAgent in spawnedAgents)
        {
            if(spawnedAgent.moveBrain.moveIdentifier == agentId)
            {
                return spawnedAgent;
            }
        }
        return null;
    }
}

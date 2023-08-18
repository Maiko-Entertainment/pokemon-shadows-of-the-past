using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public Flowchart flowchart;
    public string blockName = "Start";
    public bool onInteract = false;
    public bool repeatable = false;

    public List<WorldInteractable> cutsceneAgents = new List<WorldInteractable>();
    protected bool wasTriggered = false;
    protected bool isInteracting = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!onInteract && collision.tag == "Player" && !wasTriggered)
        {
            AddEvent();
        }
        else if (onInteract && collision.tag == "Touch" && !wasTriggered)
        {
            AddEvent();
        }
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        bool isPlaying = InteractionsMaster.GetInstance().IsInteractionPlaying();
        if (!isPlaying)
        {
            if (onInteract && collision.collider.tag == "Touch" && !wasTriggered)
            {
                AddEvent();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!onInteract && collision.tag == "Player")
        {
            isInteracting = false;
        }
    }

    public void AddEvent()
    {
        if (!repeatable)
            wasTriggered = true;
        if (!isInteracting || onInteract)
        {
            isInteracting = true;
            InteractionsMaster.GetInstance().AddEvent(new InteractionEventCutscene(this));
        }
    }

    public void Initiate()
    {
        if (flowchart)
        {
            CutsceneMaster.GetInstance().LoadCutscene(this);
            flowchart.ExecuteBlock(blockName);
        }
    }

    public WorldInteractable GetAgent(string agentId)
    {
        foreach(WorldInteractable spawnedAgent in cutsceneAgents)
        {
            if(spawnedAgent.moveBrain.moveIdentifier == agentId)
            {
                return spawnedAgent;
            }
        }
        return null;
    }
}

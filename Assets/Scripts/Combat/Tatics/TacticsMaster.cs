using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMaster : MonoBehaviour
{
    public static TacticsMaster instance;
    public List<TacticData> tactics = new List<TacticData>();
    public List<TacticData> baseTactics = new List<TacticData>();
    public List<TacticData> learntTactics = new List<TacticData>();
    public Flowchart tacticsFlochart;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Load(SaveFile save)
    {
        foreach (PersistedTactic pt in save.persistedTactics)
        {
            learntTactics.Add(GetTactic(pt.tacticId));
        }
    }
    public static TacticsMaster GetInstance()
    {
        return instance;
    }

    public TacticData GetTactic(TacticId id)
    {
        foreach(TacticData tactic in tactics)
        {
            if (tactic.tacticId == id)
                return tactic;
        }
        return null;
    }
    public List<TacticData> GetEquippedTactics()
    {
        return baseTactics;
    }
}

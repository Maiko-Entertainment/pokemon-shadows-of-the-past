using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnConditionData : MonoBehaviour
{
    public List<SpawnConditionDataSaveValue> conditions = new List<SpawnConditionDataSaveValue>();
    public List<ConditionalSpawnData> spawns = new List<ConditionalSpawnData>();

    private void Start()
    {
        CheckForSpawn();
    }

    protected bool MeetsConditions()
    {
        foreach(SpawnConditionDataSaveValue con in conditions)
        {
            if (!con.IsTrue())
                return false;
        }
        return true;
    }

    public void CheckForSpawn()
    {
        if (MeetsConditions())
        {
            foreach(ConditionalSpawnData spawn in spawns)
            {
                GameObject obj = Instantiate(spawn.spawn);
                obj.transform.localPosition = spawn.spawnPostion;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnConditionData : MonoBehaviour
{
    public List<SpawnConditionDataSaveValue> conditions = new List<SpawnConditionDataSaveValue>();
    public List<ConditionalSpawnData> spawns = new List<ConditionalSpawnData>();
    public float chance = 1f;

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
        float random = Random.value;
        print("Chance: " + chance + " - Random: " + random);
        if (MeetsConditions() && chance >= random)
        {
            foreach(ConditionalSpawnData spawn in spawns)
            {
                print("Instancing " + spawn.spawn);
                GameObject obj = Instantiate(spawn.spawn, spawn.spawnAsChild ? transform : null);
                if (spawn.spawnAsChild)
                {
                    obj.transform.localPosition = spawn.spawnPostion;
                }
                else
                {
                    obj.transform.localPosition = transform.position + spawn.spawnPostion;
                }
            }
        }
    }
}

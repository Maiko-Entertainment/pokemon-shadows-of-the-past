using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesMaster : MonoBehaviour
{
    public static MovesMaster Instance;

    protected Dictionary<string, MoveData> movesDictionary = new Dictionary<string, MoveData>();

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            LoadDictionary();
        }
    }
    public void LoadDictionary()
    {
        MoveData[] baseDatas = Resources.LoadAll<MoveData>(ResourceMaster.Instance.GetMovesDataPath());
        foreach (MoveData ad in baseDatas)
        {
            try
            {
                movesDictionary.Add(ad.GetId(), ad);
            }
            catch (Exception e)
            {
                Debug.LogError(e + " \n" + ad.name + " -> "+ movesDictionary[ad.GetId()].name);
            }
        }
    }
    public MoveData GetMove(string id)
    {
        if (movesDictionary.ContainsKey(id))
        {
            return movesDictionary[id];
        }
        return null;
    }
}

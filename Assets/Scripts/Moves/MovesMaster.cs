using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesMaster : MonoBehaviour
{
    public static MovesMaster Instance;

    protected Dictionary<MoveId, MoveData> movesDictionary = new Dictionary<MoveId, MoveData>();

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
                movesDictionary.Add(ad.moveId, ad);
            }
            catch (Exception e)
            {
                Debug.LogError(e + " \n" + ad.name + " -> "+ movesDictionary[ad.moveId].name);
            }
        }
    }
    public MoveData GetMove(MoveId id)
    {
        if (movesDictionary.ContainsKey(id))
        {
            return movesDictionary[id];
        }
        return null;
    }
}

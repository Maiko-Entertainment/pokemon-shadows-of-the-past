using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesMaster : MonoBehaviour
{
    public static MovesMaster Instance;
    public List<MoveData> movesData = new List<MoveData>();

    private void Awake()
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
    
    public MoveData GetMove(MoveId id)
    {
        foreach(MoveData m in movesData)
        {
            if (m.moveId == id)
            {
                return m;
            }
        }
        return null;
    }
}

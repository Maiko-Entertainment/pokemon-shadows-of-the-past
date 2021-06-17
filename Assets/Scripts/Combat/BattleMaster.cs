using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMaster : MonoBehaviour
{
    public static BattleMaster Instance { get; set; }

    public BattleManager currenBattle;
    public bool triggerBattleOnStart = false;

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

    private void Start()
    {
        if (triggerBattleOnStart)
        {
            RunTestBattle();
        }
    }

    public static BattleMaster GetInstance()
    {
        return Instance;
    }

    public BattleManager GetCurrentBattle()
    {
        return currenBattle;
    }

    public void RunTestBattle()
    {
        Debug.Log("Started test battle");
        GetCurrentBattle().StartBattle();
    }
}

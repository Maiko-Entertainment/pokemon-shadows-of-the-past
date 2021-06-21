using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMaster : MonoBehaviour
{
    public static BattleMaster Instance { get; set; }
    public Flowchart battleFlowchart;

    public BattleManager currenBattle;
    public bool triggerBattleOnStart = false;

    public BattleTypeAdvantageManager advantageManager = new BattleTypeAdvantageManager();

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

    public float GetAdvantageMultiplier(PokemonTypeId damageType, List<PokemonTypeId> targetTypes)
    {
        float multiplier = 1;
        foreach(PokemonTypeId targetType in targetTypes)
        {
            BattleTypeAdvantageType adv = advantageManager.GetTypeRelation(damageType, targetType);
            multiplier *= advantageManager.GetAdvantageMultiplier(adv);
        }
        return multiplier;
    }

    public TypeData GetTypeData(PokemonTypeId type)
    {
        return advantageManager.GetTypeData(type);
    }

    public Flowchart GetBattleFlowchart()
    {
        return battleFlowchart;
    }
}

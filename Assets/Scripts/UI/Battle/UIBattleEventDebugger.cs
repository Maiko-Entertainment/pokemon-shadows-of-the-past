using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIBattleEventDebugger : MonoBehaviour
{
    public static UIBattleEventDebugger Instance;
    public UIBattleEventDebuggerEvent eventPrefab;
    public Transform eventsList;

    private void Awake()
    {
        Instance = this;
    }

    public static UIBattleEventDebugger GetInstance() { return Instance; }

    public void UpdateTriggers()
    {
        foreach (Transform be in eventsList)
        {
            Destroy(be.gameObject);
        }
        BattleEventId[] bes = BattleMaster.GetInstance().GetCurrentBattle().eventManager.battleEvents.Keys.ToArray();
        foreach (BattleEventId be in bes)
        {
            Instantiate(eventPrefab.gameObject, eventsList).GetComponent<UIBattleEventDebuggerEvent>().Load(be);
        }
    }
}

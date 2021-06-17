using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIBattleEventDebuggerEvent : MonoBehaviour
{
    public Text title;
    public UiBattleEventDebuggerTrigger triggerPrefab;
    public Transform triggerList;

    protected BattleEventId eventId;

    public void Load(BattleEventId eventId)
    {
        this.eventId = eventId;
        title.text = eventId.ToString();
        foreach (Transform trigger in triggerList)
        {
            Destroy(trigger.gameObject);
        }
        List<BattleTrigger> triggers = BattleMaster.GetInstance().GetCurrentBattle().eventManager.GetTriggersForEvent(eventId);
        foreach (BattleTrigger bt in triggers)
        {
            Instantiate(triggerPrefab.gameObject, triggerList).GetComponent<UiBattleEventDebuggerTrigger>().Load(bt);
        }
    }
}

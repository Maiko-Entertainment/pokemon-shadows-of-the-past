using UnityEngine;
using UnityEngine.UI;

public class UiBattleEventDebuggerTrigger : MonoBehaviour
{
    public Text title;
    
    public void Load(BattleTrigger trigger)
    {
        title.text = trigger.ToString();
    }
}

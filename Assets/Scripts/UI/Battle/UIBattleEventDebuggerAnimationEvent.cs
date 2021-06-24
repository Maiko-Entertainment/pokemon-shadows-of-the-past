using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleEventDebuggerAnimationEvent : MonoBehaviour
{
    public Text title;

    public UIBattleEventDebuggerAnimationEvent Load(BattleAnimatorEvent animEvent)
    {
        title.text = animEvent.ToString();
        return this;
    }
}

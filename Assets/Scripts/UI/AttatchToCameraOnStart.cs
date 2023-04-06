using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttatchToCameraOnStart : MonoBehaviour
{
    public Canvas canvas;
    void Start()
    {
        if (BattleMaster.GetInstance().IsBattleActive())
        {
            canvas.worldCamera = TransitionMaster.GetInstance().battleCamera;
        }
        else
        {
            canvas.worldCamera = TransitionMaster.GetInstance().worldCamera;
        }
    }
}

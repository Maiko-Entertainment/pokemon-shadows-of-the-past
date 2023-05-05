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
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        else
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = TransitionMaster.GetInstance().worldCamera;
        }
    }
}

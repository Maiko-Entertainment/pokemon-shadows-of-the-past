using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneMaster : MonoBehaviour
{
    public static CutsceneMaster Instance;

    private Cutscene currentCutscene;

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
    public static CutsceneMaster GetInstance() { return Instance; }

    public void LoadCutscene(Cutscene cutscene)
    {
        currentCutscene = cutscene;
    }

    public Cutscene GetCurrentCutscene()
    {
        return currentCutscene;
    }
}

using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMaster : MonoBehaviour
{
    public static TransitionMaster Instance;

    public Camera sceneCamera;
    public Camera battleCamera;
    public Camera worldCamera;

    public SayDialog sceneDialogue;
    public SayDialog combatDialogue;

    public ViewTransition battleEndTransition;

    public Transform transitions;

    private bool wasInWorldBefore = false;

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
        EnableWorldCamera();
    }

    public static TransitionMaster GetInstance()
    {
        return Instance;
    }

    public void SetDialogueToScene()
    {
        if (sceneDialogue != null)
        {
            SayDialog.ActiveSayDialog = sceneDialogue;
        }
    }

    public void SetDialogueToBattle()
    {
        if (sceneDialogue != null)
        {
            SayDialog.ActiveSayDialog = combatDialogue;
        }
    }

    // Returns time it takes to cover entire screen
    public void RunPokemonBattleTransition(ViewTransition transition)
    {
        StartCoroutine(EnableBattleCameraAfter(transition.changeTime));
        Instantiate(transition.gameObject, transitions);
    }

    public void RunTrainerBattleTransition(ViewTransition transition)
    {
        StartCoroutine(EnableBattleCameraAfter(transition.changeTime));
        Instantiate(transition.gameObject, transitions);
    }

    IEnumerator EnableBattleCameraAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        SetDialogueToBattle();
        EnableBattleCamera();
    }

    public float RunSceneTransition()
    {
        StartCoroutine(EnableSceneCameraAfter(battleEndTransition.changeTime));
        Instantiate(battleEndTransition, transitions);
        return battleEndTransition.changeTime;
    }
    IEnumerator EnableSceneCameraAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        EnableSceneCamera();
        SetDialogueToScene();
    }

    public float RunWorldTransition()
    {
        
        StartCoroutine(EnableWorldCameraAfter(battleEndTransition.changeTime));
        Instantiate(battleEndTransition, transitions);
        return battleEndTransition.changeTime;
    }
    IEnumerator EnableWorldCameraAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        EnableWorldCamera();
        SetDialogueToScene();
    }

    public void DisableCameras()
    {
        sceneCamera.enabled = false;
        battleCamera.enabled = false;
        worldCamera.enabled = false;
    }

    public void EnableSceneCamera()
    {
        DisableCameras();
        wasInWorldBefore = false;
        sceneCamera.enabled = true;
    }
    public void EnableBattleCamera()
    {
        DisableCameras();
        battleCamera.enabled = true;
    }

    public void EnableWorldCamera()
    {
        DisableCameras();
        wasInWorldBefore = true;
        worldCamera.enabled = true;
    }

    public float ReturnToPreviousCamera()
    {
        if (wasInWorldBefore)
            return RunWorldTransition();
        else
            return RunSceneTransition();
    }
}

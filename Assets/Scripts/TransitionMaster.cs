﻿using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TransitionMaster : MonoBehaviour
{
    public static TransitionMaster Instance;

    public Camera sceneCamera;
    public Camera battleCamera;
    public Camera worldCamera;
    public VolumeProfile defaultVolumeProfile;

    public SayDialog sceneDialogue;
    public SayDialog combatDialogue;
    public SayDialog evolutionDialogue;

    public ViewTransition battleEndTransition;

    public Transform transitions;
    public Transform mapDayEffectsList;

    public delegate void CameraChange(Camera camera);
    public event CameraChange OnCameraChange;

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
            UpdateDialogCamera();
        }
    }

    public void SetDialogueToBattle()
    {
        if (sceneDialogue != null)
        {
            SayDialog.ActiveSayDialog = combatDialogue;
            UpdateDialogCamera();
        }
    }
    public void SetDialogueToEvolution()
    {
        if (sceneDialogue != null)
        {
            SayDialog.ActiveSayDialog = evolutionDialogue;
            UpdateDialogCamera();
        }
    }
    public void UpdateDialogCamera()
    {
        SayDialog.ActiveSayDialog.GetComponent<Canvas>().worldCamera = Camera.main;
    }
    public void RunTransition(ViewTransition transition)
    {
        Instantiate(transition.gameObject, transitions);
    }

    public void ClearTransitions()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform t in transitions)
        {
            children.Add(t);
        }
        StartCoroutine(ClearRoutine(children));
    }
    protected IEnumerator ClearRoutine(List<Transform> transitions)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        foreach (Transform t in transitions)
        {
            Destroy(t.gameObject);
        }
    }

    public void RunToWorldTransition(ViewTransition transition)
    {
        RunTransition(transition);
        StartCoroutine(EnableWorldCameraAfter(transition.changeTime));
    }

    // Returns time it takes to cover entire screen
    public void RunPokemonBattleTransition(ViewTransition transition)
    {
        UIPauseMenuMaster.GetInstance()?.HideWorldUI();
        StartCoroutine(EnableBattleCameraAfter(transition.changeTime));
        Instantiate(transition.gameObject, transitions);
    }

    public void RunTrainerBattleTransition(ViewTransition transition)
    {
        UIPauseMenuMaster.GetInstance()?.HideWorldUI();
        StartCoroutine(EnableBattleCameraAfter(transition.changeTime));
        Instantiate(transition.gameObject, transitions);
    }

    IEnumerator EnableBattleCameraAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        ClearDayEffects();
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
        UIPauseMenuMaster.GetInstance()?.ShowWorldUI();
    }

    public float RunBattleToWorldTransition()
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
        WorldMapMaster.GetInstance()?.HandleMapReturn();
        UIPauseMenuMaster.GetInstance()?.ShowWorldUI();
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
        worldCamera.GetComponent<WorldCamera>().LookForPlayer();
        SetDialogueToScene();
    }

    public float SetWorldCameraOffset(Vector3 offset)
    {
        return worldCamera.GetComponent<WorldCamera>().SetOffset(offset);
    }

    public float ReturnToPreviousCamera()
    {
        if (wasInWorldBefore)
            return RunBattleToWorldTransition();
        else
            return RunSceneTransition();
    }
    public void ClearDayEffects()
    {
        foreach(Transform previousEffect in mapDayEffectsList)
        {
            Destroy(previousEffect.gameObject);
        }
    }
    public void InitiateDayEffect(GameObject effect)
    {
        Instantiate(effect, mapDayEffectsList);
    }

    public void SetCameraProfile(VolumeProfile profile)
    {
        if (profile)
            worldCamera.GetComponent<Volume>().profile = profile;
        else
            ResetWorldCameraProfile();
    }
    public void ResetWorldCameraProfile()
    {
        worldCamera.GetComponent<Volume>().profile = defaultVolumeProfile;
    }

    public void SetBattleCameraProfile(VolumeProfile profile)
    {
        if (profile)
            battleCamera.GetComponent<Volume>().profile = profile;
        else
            ResetBattleCameraProfile();
    }
    public void ResetBattleCameraProfile()
    {
        battleCamera.GetComponent<Volume>().profile = defaultVolumeProfile;
    }
}

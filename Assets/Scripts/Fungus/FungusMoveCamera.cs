using UnityEngine;
using Fungus;
using System.Collections;

[CommandInfo(
    "Camera",
    "Move Camera",
    "Set camera offset."
)]
public class FungusMoveCamera : Command
{
    public Vector3 offset;
    public bool waitForEnd = true;
    public override void OnEnter()
    {
        float time = TransitionMaster.GetInstance().SetWorldCameraOffset(offset);
        if (waitForEnd)
        {
            StartCoroutine(ContinueAfter(time));
        }
        else
        {
            Continue();
        }
    }

    IEnumerator ContinueAfter(float time)
    {
        yield return new WaitForSeconds(time);
        Continue();
    }

    public override Color GetButtonColor()
    {
        return new Color32(42, 176, 49, 255);
    }

    public override string GetSummary()
    {
        return offset.ToString();
    }
}

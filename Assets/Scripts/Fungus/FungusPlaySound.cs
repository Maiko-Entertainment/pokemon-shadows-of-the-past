using UnityEngine;
using Fungus;
[CommandInfo(
    "Audio",
    "Play Sound from Master",
    "Plays music contacting the Audio Master."
)]
public class FungusPlaySound : Command
{
    public AudioClip clip;
    public override void OnEnter()
    {
        Continue();
        AudioMaster.GetInstance()?.PlaySfx(clip);
    }

    public override Color GetButtonColor()
    {
        return new Color32(42, 176, 49, 255);
    }
    public override string GetSummary()
    {
        string clipName = clip != null ? clip.name : "None";
        return clipName;
    }
}

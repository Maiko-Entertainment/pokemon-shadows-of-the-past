using UnityEngine;
using Fungus;
[CommandInfo(
    "Audio",
    "Play Music from Master",
    "Plays music contacting the Audio Master."
)]
public class FungusPlayMusic : Command
{
    public AudioOptions clip;
    public override void OnEnter()
    {
        Continue();
        AudioMaster.GetInstance()?.PlayMusic(clip);
    }

    public override Color GetButtonColor()
    {
        return new Color32(42, 176, 49, 255);
    }

    public override string GetSummary()
    {
        string clipName = clip != null && clip.audio ? clip.audio.name + " - Pitch: " + clip.pitch : "None";
        return clipName;
    }
}

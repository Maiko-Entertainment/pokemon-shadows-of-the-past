using UnityEngine;
using Fungus;
[CommandInfo(
    "Game",
    "Open Intro Form",
    ""
)]
public class FungusOpenIntroForm : Command
{
    public override void OnEnter()
    {
        Continue();
        UIGameIntroMaster.GetInstance().OpenPlayerDataPanel();
    }

    public override Color GetButtonColor()
    {
        return new Color32(42, 176, 49, 255);
    }

    public override string GetSummary()
    {
        return base.GetSummary();
    }
}

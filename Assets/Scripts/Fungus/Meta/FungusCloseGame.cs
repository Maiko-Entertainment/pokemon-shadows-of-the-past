using Fungus;
using UnityEngine;
[CommandInfo(
    "Game",
    "Close Game",
    "Closes the game inmediately."
)]
public class FungusCloseGame : Command
{
    public override void OnEnter()
    {
        Application.Quit();
        Continue();
    }
}

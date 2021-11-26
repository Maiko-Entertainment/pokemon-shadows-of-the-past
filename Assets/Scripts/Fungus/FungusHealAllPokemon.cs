using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Game",
    "Heal all Pokemon",
    "Heals all pokemon."
)]
public class FungusHealAllPokemon: Command
{
    public override void OnEnter()
    {
        PartyMaster.GetInstance().HealAll();
        Continue();
    }
}

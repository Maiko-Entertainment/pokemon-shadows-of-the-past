using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventCheckEvolution : BattleAnimatorEvent
{
    public BattleAnimatorEventCheckEvolution():base()
    {

    }

    public override void Execute()
    {
        // Checks for evo conditions
        foreach (PokemonCaughtData pkmn in PartyMaster.GetInstance().GetParty())
        {
            PokemonMaster.GetInstance().CheckForEvolution(pkmn);
        }
        BattleAnimatorMaster.GetInstance().GoToNextBattleAnim();
    }
}

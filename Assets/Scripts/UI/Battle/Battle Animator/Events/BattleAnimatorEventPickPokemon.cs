using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventPickPokemon : BattleAnimatorEvent
{
    public override void Execute()
    {
        BattleAnimatorMaster.GetInstance()?.ShowPokemonSelection();
    }
}

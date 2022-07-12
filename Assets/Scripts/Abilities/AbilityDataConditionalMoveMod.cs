using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/Conditional Move Mod")]
public class AbilityDataConditionalMoveMod : AbilityData
{
    public int maxPower = 200;
    public UseMoveMods moveMods; 

    public override void Initialize(PokemonBattleData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (bm != null)
        {
            BattleTriggerOnPokemonMoveConditional conTrigger = new BattleTriggerOnPokemonMoveConditional(pokemon, moveMods, true);
            conTrigger.maxPower = maxPower;
            bm.AddTrigger(conTrigger);
        }
    }
}

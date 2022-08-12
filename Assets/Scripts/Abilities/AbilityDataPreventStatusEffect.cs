using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/Prevent Status Effects")]
public class AbilityDataPreventStatusEffect : AbilityData
{
    public List<StatusEffectId> statusPrevented = new List<StatusEffectId>();

    public override void Initialize(PokemonBattleData pokemon)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (bm != null)
        {
            BattleTriggerOnPokemonStatusPrevent conTrigger = new BattleTriggerOnPokemonStatusPrevent(pokemon, statusPrevented, true);
            bm.AddTrigger(conTrigger);
        }
    }
}

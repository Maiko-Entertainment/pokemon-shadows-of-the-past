using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/Copy Status Back")]
public class AbilityDataGiveBackStatus : AbilityData
{
    public List<StatusEffectId> applicableStatus;
    public override void Initialize(PokemonBattleData pokemon)
    {
        BattleTriggerOnPokemonStatusGiveBack trigger = new BattleTriggerOnPokemonStatusGiveBack(pokemon, applicableStatus, true);
        BattleMaster.GetInstance().GetCurrentBattle().AddTrigger(trigger);
        base.Initialize(pokemon);
    }
}

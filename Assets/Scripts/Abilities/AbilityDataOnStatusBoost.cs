using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/On Status Move Boost")]
public class AbilityDataOnStatusBoost : AbilityData
{
    public List<StatusEffectId> includedStatus = new List<StatusEffectId>();
    public UseMoveMods mods = new UseMoveMods(PokemonTypeId.Unmodify);

    public override void Initialize(PokemonBattleData pokemon)
    {
        BattleTriggerOnPokemonMoveStatusMod eventMod = new BattleTriggerOnPokemonMoveStatusMod(pokemon, mods, includedStatus, true);
        BattleMaster.GetInstance().GetCurrentBattle().AddTrigger(eventMod);
        base.Initialize(pokemon);
    }
}

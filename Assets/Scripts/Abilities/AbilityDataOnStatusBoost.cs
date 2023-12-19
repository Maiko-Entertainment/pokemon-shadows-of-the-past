using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/On Status Move Boost")]
public class AbilityDataOnStatusBoost : AbilityData
{
    public List<StatusEffectData> includedStatus = new List<StatusEffectData>();
    public List<StatusFieldData> includedFieldStatus = new List<StatusFieldData>();
    public UseMoveMods mods = new UseMoveMods(null);

    public override void Initialize(PokemonBattleData pokemon)
    {
        BattleTriggerOnPokemonMoveStatusMod eventMod = new BattleTriggerOnPokemonMoveStatusMod(pokemon, mods, includedStatus, true);
        BattleMaster.GetInstance().GetCurrentBattle().AddTrigger(eventMod);
        base.Initialize(pokemon);
    }
}

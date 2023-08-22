using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/Grant Inmunity")]
public class AbilityDataGrantInmunity : AbilityData
{
    public List<PokemonTypeId> inmuneTypes = new List<PokemonTypeId>();
    public override void Initialize(PokemonBattleData pokemon)
    {
        base.Initialize(pokemon);
        UseMoveMods mods = new UseMoveMods(PokemonTypeId.Unmodify);
        mods.powerMultiplier *= 0;
        BattleTriggerOnPokemonTakeMove trigger = new BattleTriggerOnPokemonTakeMove(
                pokemon,
                mods,
                inmuneTypes,
                true
            );
        trigger.grantsInmunite = true;
        BattleMaster.GetInstance().GetCurrentBattle().AddTrigger(trigger);
    }
}

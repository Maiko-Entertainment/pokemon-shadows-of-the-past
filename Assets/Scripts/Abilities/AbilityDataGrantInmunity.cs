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
        BattleTriggerOnPokemonTakeMove trigger = new BattleTriggerOnPokemonTakeMove(
                pokemon,
                null,
                inmuneTypes,
                true
            );
        BattleMaster.GetInstance().GetCurrentBattle().AddTrigger(trigger);
    }
}

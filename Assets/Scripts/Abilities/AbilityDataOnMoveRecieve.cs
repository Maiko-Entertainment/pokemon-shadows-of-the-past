using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/On Pokemon Take Move")]
public class AbilityDataOnMoveRecieve : AbilityData
{
    public List<TypeData> affectedTypes = new List<TypeData>();
    public bool grantsInmunity = false;
    public UseMoveMods moveMods = new UseMoveMods(null);

    public List<MoveStatusChance> statusChances = new List<MoveStatusChance>();
    public List<MoveStatChange> statChanges = new List<MoveStatChange>();
    public override void Initialize(PokemonBattleData pokemon)
    {
        base.Initialize(pokemon);
        BattleTriggerOnPokemonTakeMove trigger = new BattleTriggerOnPokemonTakeMove(
                pokemon,
                moveMods,
                true
            );
        trigger.grantsInmunity = grantsInmunity;
        trigger.affectedTypes = affectedTypes;
        trigger.statusChances = statusChances;
        trigger.statChanges = statChanges;
        BattleMaster.GetInstance().GetCurrentBattle().AddTrigger(trigger);
    }
}

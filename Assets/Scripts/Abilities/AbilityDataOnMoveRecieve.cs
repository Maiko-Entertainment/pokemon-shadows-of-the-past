using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/On Pokemon Take Move")]
public class AbilityDataOnMoveRecieve : AbilityData
{
    public List<PokemonTypeId> affectedTypes = new List<PokemonTypeId>();
    public bool grantsInmunity = false;
    public UseMoveMods moveMods = new UseMoveMods(PokemonTypeId.Unmodify);

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

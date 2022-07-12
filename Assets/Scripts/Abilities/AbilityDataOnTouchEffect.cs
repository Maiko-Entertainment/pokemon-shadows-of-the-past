using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/OnTouchEffect")]
public class AbilityDataOnTouchEffect : AbilityData
{
    public List<DamageSummarySource> damageSources = new List<DamageSummarySource>() { DamageSummarySource.Move };
    public bool triggerOnMoveContact = true;
    public bool onlyTriggerOnOppositeGender = false;
    public List<MoveStatusChance> statusChances = new List<MoveStatusChance>();
    public List<MoveStatChange> moveStatChanges = new List<MoveStatChange>();
    public override void Initialize(PokemonBattleData pokemon)
    {
        base.Initialize(pokemon);
        BattleTriggerOnDamageEffect trigger = new BattleTriggerOnDamageEffect(pokemon, statusChances, moveStatChanges);
        trigger.damageSources = damageSources;
        trigger.isAbilitySource = true;
        trigger.requiresContact = triggerOnMoveContact;
        trigger.workOnlyOnOppositeGender = onlyTriggerOnOppositeGender;
        BattleMaster.GetInstance().GetCurrentBattle().AddTrigger(trigger);
    }
}

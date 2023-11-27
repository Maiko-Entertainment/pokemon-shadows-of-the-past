using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/OnTakeDamage")]
public class AbilityDataOnTakeDamageEffect : AbilityData
{
    public List<DamageSummarySource> damageSources = new List<DamageSummarySource>() { DamageSummarySource.Move };
    public bool triggerOnMoveContact = true;
    public bool onlyTriggerOnOppositeGender = false;
    public List<MoveStatusChance> statusChances = new List<MoveStatusChance>();
    public List<MoveStatChange> moveStatChanges = new List<MoveStatChange>();
    public List<TypeData> affectedMoveTypes = new List<TypeData>();
    public override void Initialize(PokemonBattleData pokemon)
    {
        base.Initialize(pokemon);
        BattleTriggerOnDamageEffect trigger = new BattleTriggerOnDamageEffect(pokemon, statusChances, moveStatChanges);
        trigger.damageSources = damageSources;
        trigger.isAbilitySource = true;
        trigger.requiresContact = triggerOnMoveContact;
        trigger.workOnlyOnOppositeGender = onlyTriggerOnOppositeGender;
        trigger.affectedMoveTypes = affectedMoveTypes;
        BattleMaster.GetInstance().GetCurrentBattle().AddTrigger(trigger);
    }
}

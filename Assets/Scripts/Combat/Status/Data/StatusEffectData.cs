using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Status/Pokemon/Simple")]
public class StatusEffectData : StatusData
{
    [Range(0f, 1f)]
    public float healOpponentsFromDamage = 0f;
    public float multiplyDamageEachTurn = 1f;
    public bool isPrimary = false;
    public List<BattleAnimationPokemon> hitAnims;
    public bool tickDownAtEndOfRoundInstead = false;

    public int captureRateBonus = 0;
    public List<TypeData> inmuneTypes = new List<TypeData>();
    public PokemonBattleStatsMultiplier statsMultiplier = new PokemonBattleStatsMultiplier();

    [Header("Cancel or Replace Move Config")]
    [Range(0f, 1f)]
    public float cancelMoveChance = 0f;
    // Useful for status like confusion which trigger animation even if status doesnt trigger
    public bool playAnimationEvenIfMoveIsntCanceled = false;
    public MoveData replaceMoveInsteadOfCancel;

    [Header("Damage modifiers for ALL sources")]
    [Tooltip("Use to modify ALL damage the pokemon takes (enemy) or deals (any other)")]
    public List<DamageTriggerCondition> damageModifiers = new List<DamageTriggerCondition>();

    public virtual StatusEffect CreateStatusInstance(PokemonBattleData pokemon, MoveData creator)
    {
        return CreateStatusInstance(pokemon);
    }

    public virtual StatusEffect CreateStatusInstance(PokemonBattleData pokemon)
    {
        Flowchart flowchartInstance = flowchart ? Instantiate(flowchart) : null;
        StatusEffect se = new StatusEffect(pokemon, this, flowchartInstance);
        se.minTurns = minTurnDuration;
        se.addedRangeTurns = extraTurnRange;

        if (percentageDamagePerRound > 0f)
        {
            // Setup Damage
            DamageSummary damageSummary =
                new DamageSummary(
                    TypesMaster.Instance.GetTypeDataNone(),
                    Mathf.Max(1, (int)(pokemon.GetMaxHealth() * percentageDamagePerRound)),
                    DamageSummarySource.Status,
                    GetId()
                );
            damageSummary.healOpponentByDamage = healOpponentsFromDamage;
            BattleTriggerRoundEndDamage damageEvent = new BattleTriggerRoundEndDamage(
                pokemon,
                damageSummary
            );
            damageEvent.multiplierByTimesTriggered = multiplyDamageEachTurn;

            // Animation
            BattleTrigger animTrigger = new BattleTriggerOnPokemonRoundEndAnimations(
                           pokemon,
                           pokemon,
                           hitAnims
                       );
            BattleTrigger messageTrigger = new BattleTriggerOnPokemonTurnEndMessage(
                        pokemon,
                        new BattleTriggerMessageData(
                            flowchartInstance,
                            onTriggerFlowchartBlock,
                            new Dictionary<string, string>()
                            {
                            { "pokemon", pokemon.GetName() }
                            }
                        )
                    );
            // Add Triggers
            se.AddBattleTrigger(damageEvent);
            se.AddBattleTrigger(animTrigger);
            se.AddBattleTrigger(messageTrigger);
        }
        if (cancelMoveChance > 0f)
        {
            BattleTriggerOnMoveCancelChance chanceToCancelMove = new BattleTriggerOnMoveCancelChance(pokemon, cancelMoveChance);
            chanceToCancelMove.alwaysPlayAnimation = playAnimationEvenIfMoveIsntCanceled;
            chanceToCancelMove.useThisMoveInstead = replaceMoveInsteadOfCancel;
            chanceToCancelMove.flowchart = flowchart;
            chanceToCancelMove.blockName = onTriggerFlowchartBlock;
            chanceToCancelMove.animations = hitAnims;
            se.AddBattleTrigger(chanceToCancelMove);
        }
        // Setup damage triggers that modify damage taken or dealt and adds them to the trigger list
        foreach(DamageTriggerCondition damageTriggerCon in damageModifiers)
        {
            BattleTriggerBeforeDamage damageTrigger;
            if (damageTriggerCon.targetDamageTakenInstead)
            {
                damageTrigger = new BattleTriggerBeforeDamage(pokemon, damageTriggerCon.damageMods.DeepClone(), damageTriggerCon.triggerCondition);
            }
            else
            {
                damageTrigger = new BattleTriggerPokemonDamageDeal(pokemon, damageTriggerCon.damageMods.DeepClone(), damageTriggerCon.triggerCondition);
            }
            se.AddBattleTrigger(damageTrigger);
        }

        return se;
    }

    public void HandleVisualStart(StatusEffect se)
    {
        PokemonBattleData pokemon = se.pokemon;
        foreach (BattleAnimationPokemon anim in hitAnims)
        {
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventAnimation(pokemon, pokemon, anim));
        }
        Flowchart flowchart = BattleAnimatorMaster.GetInstance().battleFlowchart;
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(
            new BattleTriggerMessageData(
                flowchart,
                onStartStatusBlockName,
                new Dictionary<string, string> { { "pokemon", pokemon.GetName() } }
            )));
    }

    public List<TypeData> GetInmuneTypes()
    {
        return new List<TypeData>(inmuneTypes);
    }

    public string GetId()
    {
        return id;
    }
}

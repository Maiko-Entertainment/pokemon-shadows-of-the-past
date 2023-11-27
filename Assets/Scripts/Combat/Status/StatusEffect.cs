using Fungus;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StatusEffect : Status
{
    public PokemonBattleData pokemon;
    public StatusEffectData effectData;
    public List<TypeData> InmuneTypes {
        get { return effectData?.GetInmuneTypes() ?? new List<TypeData>(); }
    }
    public bool IsPrimary {
        get { return effectData?.isPrimary ?? false; }
    }
    protected bool tickDownAtEndOfRound = false;

    public StatusEffect(PokemonBattleData pokemon, StatusEffectData effectData, Flowchart flowchartInstance) : base()
    {
        this.pokemon = pokemon;
        this.effectData = effectData;
        this.flowchartInstance = flowchartInstance;
    }

    public virtual StatusEffect Copy(PokemonBattleData newPokeInstance)
    {
        StatusEffect newSe = new StatusEffect(newPokeInstance, effectData, flowchartInstance);
        newSe.effectId = effectId;
        newSe.InmuneTypes.AddRange(InmuneTypes);
        newSe.minTurns = minTurns;
        newSe.addedRangeTurns = addedRangeTurns;
        newSe.stopEscape = stopEscape;
        return newSe;
    }

    public override void Initiate()
    {
        base.Initiate();
        // Add trigger to handle status turns left reduction
        // Statuses that inherited from this should repeat this behaviour
        // Some status tick down at the end of the round instead of after a the pokemons turn
        BattleTrigger btDrop = effectData.tickDownAtEndOfRoundInstead ?
            new BattleTriggerOnRoundEndStatusDrop(this) : 
            new BattleTriggerOnDesitionStatusDrop(
                this,
                BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(pokemon)
            );
        battleTriggers.Add(btDrop);

        foreach (BattleTrigger bt in battleTriggers)
        {
            BattleMaster.GetInstance()?
                .GetCurrentBattle()?.AddTrigger(
                    bt
                );
        }
        effectData.HandleVisualStart(this);
    }

    public virtual void HandleOwnRemove()
    {
        BattleMaster.GetInstance().GetCurrentBattle().AddEvent(new BattleEventPokemonStatusRemove(pokemon, effectData));
    }

    public int GetCaptureRateBonus()
    {
        return effectData.captureRateBonus;
    }
    public override void PassTurn()
    {
        if (IsOver())
        {
            HandleOwnRemove();
        }
        else
        {
            turnsLeft -= 1;
        }
    }

    public override void Remove()
    {
        if (onEndFlowchartBlock != "" && !pokemon.IsFainted())
        {
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(new BattleTriggerMessageData(flowchartInstance, onEndFlowchartBlock, new Dictionary<string, string>() { { "pokemon", pokemon.GetName() } })));
            // TODO: Create event that destroys flowchart at the end of the animation
        }
        foreach (BattleTrigger bt in battleTriggers)
            BattleMaster.GetInstance()?.GetCurrentBattle()?.RemoveTrigger(bt);
        turnsLeft = 0;
    }

    public void AddBattleTrigger(BattleTrigger bt)
    {
        battleTriggers.Add(bt);
    }

    public override string ToString()
    {
        return "" + effectId + " on " +pokemon.GetName()+ " - Left: " + turnsLeft;
    }
}

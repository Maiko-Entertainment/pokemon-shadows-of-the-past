using Fungus;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StatusEffect : Status
{
    public PokemonBattleData pokemon;
    public StatusEffectData effectData;
    public MoveData relatedMove = null; // Almost always empty
    public List<TypeData> InmuneTypes {
        get { return effectData?.GetInmuneTypes() ?? new List<TypeData>(); }
    }
    public bool IsPrimary {
        get { return effectData?.isPrimary ?? false; }
    }
    protected bool tickDownAtEndOfRound = false;

    public StatusEffect(PokemonBattleData pokemon, StatusEffectData effectData, Flowchart flowchartInstance) : base(flowchartInstance)
    {
        this.pokemon = pokemon;
        this.effectData = effectData;
    }

    public virtual StatusEffect Copy(PokemonBattleData newPokeInstance)
    {
        StatusEffect newSe = new StatusEffect(newPokeInstance, effectData, flowchartInstance);
        newSe.InmuneTypes.AddRange(InmuneTypes);
        newSe.minTurns = minTurns;
        newSe.addedRangeTurns = addedRangeTurns;
        newSe.stopEscape = stopEscape;
        return newSe;
    }

    public override void Initiate()
    {
        // Add trigger to handle status turns left reduction
        // Statuses that inherited from this should repeat this behaviour
        // Some status tick down at the end of the round instead of after a the pokemons turn
        BattleTrigger btDrop = effectData.tickDownAtEndOfRoundInstead ?
            new BattleTriggerOnRoundEndStatusDrop(this) : 
            new BattleTriggerOnDesitionStatusDrop(
                this,
                BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(pokemon)
            );
        _battleTriggers.Add(btDrop);
        effectData.HandleVisualStart(this);

        base.Initiate();
    }

    public virtual void HandleOwnRemove()
    {
        BattleMaster.GetInstance().GetCurrentBattle().AddEvent(new BattleEventPokemonStatusRemove(pokemon, effectData));
    }

    public int GetCaptureRateBonus()
    {
        return effectData.captureRateBonus;
    }
    public override void PassRound()
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
        }
        base.Remove();
    }

    public override string ToString()
    {
        return "" + effectData.id + " on " +pokemon.GetName()+ " - Left: " + turnsLeft;
    }
}

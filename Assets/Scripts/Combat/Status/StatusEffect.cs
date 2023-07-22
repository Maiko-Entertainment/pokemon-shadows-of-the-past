using Fungus;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StatusEffect: Status
{
    public bool isPrimary;
    public PokemonBattleData pokemon;
    public int captureRateBonus = 0;
    public List<PokemonTypeId> inmuneTypes = new List<PokemonTypeId>();
    public string gainStatusBlockName;

    public StatusEffect(PokemonBattleData pokemon): base()
    {
        this.pokemon = pokemon;
    }

    public virtual StatusEffect Copy(PokemonBattleData newPokeInstance)
    {
        StatusEffect newSe = new StatusEffect(newPokeInstance);
        newSe.effectId = effectId;
        newSe.isPrimary = isPrimary;
        newSe.captureRateBonus = captureRateBonus;
        newSe.inmuneTypes.AddRange(inmuneTypes);
        newSe.minTurns = minTurns;
        newSe.addedRangeTurns = addedRangeTurns;
        newSe.stopEscape = stopEscape;
        return newSe;
    }

    public override void Initiate()
    {
        turnsLeft = minTurns + Random.Range(0, addedRangeTurns);
        // Add trigger to handle status turns left reduction
        // Statuses that inherited from this should repeat this behaviour
        BattleTrigger btDrop = new BattleTriggerOnDesitionStatusDrop(
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
    }

    public virtual void HandleOwnRemove()
    {
        BattleMaster.GetInstance().GetCurrentBattle().AddEvent(new BattleEventPokemonStatusRemove(pokemon, effectId));
    }

    public int GetCaptureRateBonus()
    {
        return captureRateBonus;
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
            Flowchart battleFlow = BattleAnimatorMaster.GetInstance().battleFlowchart;
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(new BattleTriggerMessageData(battleFlow, onEndFlowchartBlock, new Dictionary<string, string>() { { "pokemon", pokemon.GetName() } })));
        }
        foreach (BattleTrigger bt in battleTriggers)
            BattleMaster.GetInstance()?.GetCurrentBattle()?.RemoveTrigger(bt);
        turnsLeft = 0;
    }

    public override string ToString()
    {
        return "" + effectId + " on " +pokemon.GetName()+ " - Left: " + turnsLeft;
    }
}

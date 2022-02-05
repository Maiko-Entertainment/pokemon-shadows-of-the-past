using Fungus;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect: Status
{
    public bool isPrimary;
    public PokemonBattleData pokemon;
    public int captureRateBonus = 0;
    public List<PokemonTypeId> inmuneTypes = new List<PokemonTypeId>();

    public StatusEffect(PokemonBattleData pokemon, Flowchart message): base(message)
    {
        this.pokemon = pokemon;
    }

    public override void Initiate()
    {
        turnsLeft = minTurns + Random.Range(0, addedRangeTurns);
        // Add trigger to handle status turns left reduction
        // Statuses that inherited from this should repeat this behaviour
        BattleMaster.GetInstance()?.GetCurrentBattle()?.AddTrigger(
            new BattleTriggerOnDesitionStatusDrop(
                this,
                BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(pokemon)
            )
        );
    }

    public virtual void HandleOwnRemove()
    {
        BattleMaster.GetInstance().GetCurrentBattle().AddEvent(new BattleEventPokemonStatusRemove(pokemon, effectId));
    }

    public int GetCaptureRateBonus()
    {
        return captureRateBonus;
    }

    public override void Remove()
    {
        base.Remove();
    }

    public override string ToString()
    {
        return "" + effectId + "- Left: " + turnsLeft;
    }
}

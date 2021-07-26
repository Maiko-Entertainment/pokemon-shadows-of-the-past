using Fungus;
using UnityEngine;

public class StatusEffect: Status
{
    public bool isPrimary;
    public PokemonBattleData pokemon;
    public Flowchart message;
    public int captureRateBonus = 0;

    public StatusEffect(PokemonBattleData pokemon, Flowchart message)
    {
        this.pokemon = pokemon;
        this.message = message;
    }

    public override void Initiate()
    {
        turnsLeft = minTurns + Random.Range(0, addedRangeTurns);
        // Add trigger to handle status turns left reduction
        // Statuses that inherited from this should repeat this behaviour
        BattleMaster.GetInstance()?.GetCurrentBattle()?.AddTrigger(
            new BattleTriggerOnPokemonRoundEndReduceTurnsLeft(
                pokemon,
                this
            )
        );
    }

    public int GetCaptureRateBonus()
    {
        return captureRateBonus;
    }

    public override string ToString()
    {
        return "" + effectId + "- Left: " + turnsLeft;
    }
}

using Fungus;
using UnityEngine;

public class StatusEffect: Status
{
    public bool isPrimary;
    public PokemonBattleData pokemon;
    public Flowchart message;

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

    public override string ToString()
    {
        return "" + effectId + "- Left: " + turnsLeft;
    }
}

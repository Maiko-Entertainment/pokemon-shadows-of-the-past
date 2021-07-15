using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonHeal : BattleEventPokemon
{
    public HealSummary summary;

    public BattleEventPokemonHeal(PokemonBattleData pokemon, HealSummary summary) :
        base(pokemon)
    {
        this.summary = summary;
        eventId = BattleEventId.pokemonRecoverHealth;
    }

    public override void Execute()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        BattleTeamId teamId = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(pokemon);
        int resultingHealth = bm.HealPokemon(pokemon, summary);
        if (bm.GetTeamActivePokemon(teamId) == pokemon)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPokemonHeal(this, resultingHealth));
        }
        
        base.Execute();
    }
}

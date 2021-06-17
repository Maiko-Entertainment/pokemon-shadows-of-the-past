using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventTakeDamage : BattleEventPokemon
{
    public DamageSummary damageSummary;
    public BattleEventTakeDamage(PokemonBattleData pokemon, DamageSummary summary):base(pokemon)
    {
        eventId = BattleEventId.pokemonTakeDamage;
        damageSummary = summary;
    }

    public override void Execute()
    {
        base.Execute();
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        bm.ApplyDamage(this);
    }
}

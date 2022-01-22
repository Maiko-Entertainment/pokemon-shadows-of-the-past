using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventPokemonStatusRemove : BattleEventPokemon
{
    public StatusEffectId id;
    public BattleEventPokemonStatusRemove(PokemonBattleData pokemon, StatusEffectId id): base(pokemon)
    {
        this.id = id;
        eventId = BattleEventId.pokemonRemoveStatus;
    }

    public override void Execute()
    {
        pokemon.RemoveStatusEffect(id);
        BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPokemonGainStatus(pokemon));
    }
}

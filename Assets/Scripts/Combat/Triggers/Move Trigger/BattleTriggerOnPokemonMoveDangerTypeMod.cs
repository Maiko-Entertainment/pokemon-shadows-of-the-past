using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonMoveDangerTypeMod : BattleTriggerOnPokemonMove
{
    PokemonTypeId moveType;
    float healthTarget = 0.3f;
    bool showAbility = false;

    public BattleTriggerOnPokemonMoveDangerTypeMod(PokemonBattleData pokemon, UseMoveMods useMoveMods, PokemonTypeId moveType, bool showAbility) : base(pokemon, useMoveMods, true)
    {
        this.moveType = moveType;
        eventId = BattleEventId.pokemonUseMove;
        this.showAbility = showAbility;
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {
        float healthPercentage = pokemon.GetPokemonCurrentHealth() / (float)pokemon.GetPokemonHealth();
        Debug.Log(pokemon.GetName() + " - cheking for power up for " + moveType.ToString() + " moves.");
        if (pokemon == battleEvent.pokemon && battleEvent.move.GetAttackCategory() != MoveCategoryId.status && healthPercentage <= healthTarget && battleEvent.moveMods.moveTypeId == moveType)
        {
            Debug.Log(pokemon.GetName() + " - is having it's " + moveType.ToString() + " moves power up.");
            battleEvent.moveMods.Implement(useMoveMods);
            if (showAbility)
            {
                BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorAbility(pokemon));
            }
        }
        return base.Execute(battleEvent);
    }
}

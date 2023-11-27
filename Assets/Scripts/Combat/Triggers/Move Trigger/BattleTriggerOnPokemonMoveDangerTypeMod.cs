using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonMoveDangerTypeMod : BattleTriggerOnPokemonMove
{
    public List<TypeData> moveTypes;
    public float healthTarget = 0.3f;
    bool showAbility = false;

    public BattleTriggerOnPokemonMoveDangerTypeMod(PokemonBattleData pokemon, UseMoveMods useMoveMods, List<TypeData> moveTypes, bool showAbility) : base(pokemon, useMoveMods, true)
    {
        this.moveTypes = moveTypes;
        eventId = BattleEventId.pokemonUseMove;
        this.showAbility = showAbility;
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {
        float healthPercentage = pokemon.GetPokemonCurrentHealth() / (float)pokemon.GetMaxHealth();
        if (pokemon == battleEvent.pokemon && 
            battleEvent.move.GetAttackCategory() != MoveCategoryId.status && 
            healthPercentage <= healthTarget &&
            moveTypes.Contains(battleEvent.moveMods.moveType
        ))
        {
            battleEvent.moveMods.Implement(useMoveMods);
            if (showAbility)
            {
                BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorAbility(pokemon));
            }
        }
        return true;
    }
}

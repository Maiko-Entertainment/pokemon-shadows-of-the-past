using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonTakeMove : BattleTriggerOnPokemonMove
{
    public List<PokemonTypeId> affectedTypes = new List<PokemonTypeId>();
    public bool grantsInmunite = false;
    public bool showAbility = false;
    public BattleTriggerOnPokemonTakeMove(PokemonBattleData pokemon, UseMoveMods moveMods = null, List<PokemonTypeId> affectedTypes = null, bool showAbility = false):
        base(pokemon, moveMods, true)
    {
        if (affectedTypes != null)
        {
            this.affectedTypes = affectedTypes;
            this.showAbility = showAbility;
        }
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {
        PokemonBattleData target = BattleMaster.GetInstance().GetCurrentBattle().GetTarget(battleEvent.pokemon, battleEvent.move.targetType);
        if (target.battleId == pokemon.battleId && maxTriggers > 0)
        {
            bool isApplicable = true;
            if (affectedTypes.Count > 0)
            {
                if (!affectedTypes.Contains(battleEvent.move.typeId)){
                    isApplicable = false;
                }
            }
            if (isApplicable)
            {
                if (showAbility)
                {
                    BattleMaster.GetInstance().GetCurrentBattle().AddAbilityEvent(target);
                }
                if (grantsInmunite)
                {
                    BattleAnimatorMaster.GetInstance().AddEventInmuneTextEvent();
                }
                if (useMoveMods != null)
                {
                    battleEvent.moveMods.Implement(useMoveMods);
                }
            }
            else
            {
                maxTriggers++;
            }
        }
        else
        {
            maxTriggers++;
        }
        return base.Execute(battleEvent);
    }
}

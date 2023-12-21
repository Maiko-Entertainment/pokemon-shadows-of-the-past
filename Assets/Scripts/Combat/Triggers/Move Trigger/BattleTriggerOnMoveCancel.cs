using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cancels the first move it finds that targets the user
public class BattleTriggerOnMoveCancel : BattleTriggerOnPokemonMove
{
    public List<MoveCategoryId> protectedCategories = new List<MoveCategoryId>()
        {  MoveCategoryId.physical, MoveCategoryId.special, MoveCategoryId.status };
    public MoveTarget moveTarget = MoveTarget.Enemy;
    public List<MoveTags> moveTags = new List<MoveTags>();

    public float chance = 1f;
    public Dictionary<string, string> extraVariables = new Dictionary<string, string>();
    public BattleTriggerOnMoveCancel(PokemonBattleData pokemon): base(pokemon, null, true)
    {
        maxTriggers = 1;
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {
        MoveData move = battleEvent.move;
        PokemonBattleData target = BattleMaster.GetInstance().GetCurrentBattle().GetTarget(battleEvent.pokemon, move.targetType);
        bool matchesCategory = protectedCategories.Contains(move.GetAttackCategory());
        bool matchesTags = moveTags.Count == 0;
        bool meetsChance = Random.value <= chance;
        /* TODO: Redo once tags are scriptable objects
         * foreach(MoveTags tag in move.tags)
        {
            if (moveTags.Contains(tag))
            {
                matchesTags = true;
                break;
            }
        }*/
        if (matchesCategory &&
            matchesTags &&
            meetsChance &&
            moveTarget == battleEvent.move.targetType &&
            target.battleId == pokemon.battleId &&
            maxTriggers > 0)
        {
            if (blockName != "")
            {
                Dictionary<string, string> finalVars = new Dictionary<string, string>(extraVariables);
                finalVars.Add("move", move.moveName);
                finalVars.Add("pokemon", battleEvent.pokemon.GetName());
                finalVars.Add("pokemonUser", pokemon.GetName());
                BattleAnimatorMaster.GetInstance().AddEvent(
                    new BattleAnimatorEventNarrative(
                        new BattleTriggerMessageData(
                            BattleAnimatorMaster.GetInstance().battleFlowchart,
                            blockName,
                            extraVariables
                        )
                    )
                );
            }
            return false;
        }
        else
        {
            maxTriggers++;
        }
        return base.Execute(battleEvent);
    }
}

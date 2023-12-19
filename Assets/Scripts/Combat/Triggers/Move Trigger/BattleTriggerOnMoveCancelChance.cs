using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnMoveCancelChance : BattleTriggerOnPokemonMove
{
    public float chance = .25f;
    public float speedMod = 0.5f;
    public bool alwaysPlayAnimation = false;
    public MoveData useThisMoveInstead;

    public BattleTriggerOnMoveCancelChance(PokemonBattleData pokemon, float chance) : base(pokemon, new UseMoveMods(null), true)
    {
        priority = 15;
        this.chance = chance;
    }

    public override bool Execute(BattleEventUseMove battleEvent)
    {
        float random = Random.value;
        
        if (pokemon == battleEvent.pokemon)
        {
            bool chanceTriggers = random < chance;
            // Status like confusion always trigger their animation even if it doesn't trigger
            if (chanceTriggers || alwaysPlayAnimation)
            {
                foreach(BattleAnimationPokemon anim in animations)
                {
                    BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventAnimation(pokemon, pokemon, anim));
                }
                if (flowchart && !string.IsNullOrEmpty(blockName))
                {
                    Flowchart flowchart = BattleAnimatorMaster.GetInstance().battleFlowchart;
                    BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(
                        new BattleTriggerMessageData(
                            flowchart,
                            blockName,
                            new Dictionary<string, string> { { "pokemon", pokemon.GetName() } }
                        )));
                }
            }
            if (chanceTriggers)
            {
                // If we dont have a move to replace it then we cancel the move
                if (!useThisMoveInstead)
                {
                    BattleMaster.GetInstance().GetCurrentBattle().AddEvent(new BattleEventUseMoveFail(battleEvent));
                    return false;
                }
                else
                {
                    // If not then we replace the move
                    battleEvent.move = useThisMoveInstead;
                    battleEvent.moveMods.moveType = useThisMoveInstead.GetMoveType();
                }
            }
        }
        return base.Execute(battleEvent);
    }
}

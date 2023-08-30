using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Moves/Multihit MoveData")]
public class MoveDataMultihit : MoveData
{
    public int maxHits = 5;
    public float extraHitChance = 0.5f;
    public List<BattleAnimation> beginAnimation = new List<BattleAnimation>();
    public List<BattleAnimation> endAnimation = new List<BattleAnimation>();
    public override void Execute(BattleEventUseMove battleEvent)
    {
        int randomHits = 2;
        for(; randomHits < maxHits; randomHits++){
            float random = Random.value;
            if (extraHitChance < random)
            {
                break;
            }
            randomHits++;
        }
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        battleEvent.pokemon.ReduceMovePP(this);
        PokemonBattleData pokemonTarget = bm.GetTarget(battleEvent.pokemon, battleEvent.move.targetType);
        bool moveHits = bm.CheckForMoveHit(battleEvent);

        // Tell is using move and first anims
        BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventPokemonMoveFlowchart(battleEvent));
        // Self targeting moves dont usually miss
        if (moveHits || targetType == MoveTarget.Self)
        {
            foreach (BattleAnimation anim in beginAnimation)
            {
                BattleAnimatorMaster.GetInstance()?.AddEvent(
                    new BattleAnimatorEventPokemonMoveAnimation(battleEvent.pokemon, pokemonTarget, anim)
                );
            }
            List<BattleAnimation> reverseList = new List<BattleAnimation>(endAnimation);
            reverseList.Reverse();
            foreach (BattleAnimation anim in reverseList)
            {
                BattleMaster.GetInstance()?.GetCurrentBattle().AddEvent(
                    new BattleEventAnimation(battleEvent.pokemon, pokemonTarget, anim)
                );
            }
            while (randomHits > 0)
            {

                if (categoryId != MoveCategoryId.status)
                {
                    DamageSummary damageSummary = bm.CalculateMoveDamage(battleEvent);
                    bm.AddDamageDealtEvent(pokemonTarget, damageSummary);
                }
                else
                {
                    HandleStatsChanges(battleEvent.pokemon);
                    HandleStatusAdds(battleEvent.pokemon);
                    HandleDestroy(pokemonTarget, battleEvent.pokemon);
                    HandleAnimations(battleEvent.pokemon, pokemonTarget);
                }
                randomHits--;
            }
            // Negative values are used for recoil
            if (drainMultiplier != 0 && moveHits)
            {
                BattleMaster.GetInstance().GetCurrentBattle()?.AddTrigger(new BattleTriggerDrainOnMoveDamage(battleEvent.pokemon, this, drainMultiplier));
            }
            bm.AddMoveSuccessEvent(battleEvent);
        }
        else
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(new BattleAnimatorEventMoveMiss(battleEvent));
        }
    }
}

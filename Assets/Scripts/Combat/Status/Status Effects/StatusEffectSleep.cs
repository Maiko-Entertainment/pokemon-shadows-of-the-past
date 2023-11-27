using Fungus;
using System.Collections.Generic;
[System.Serializable]

public class StatusEffectSleep : StatusEffect
{
    public StatusEffectSleep(PokemonBattleData pokemon) : base(pokemon, null, null)
    {
        effectId = StatusEffectId.Sleep;
        minTurns = 1;
        addedRangeTurns = 2;
        // captureRateBonus = 30;
        onEndFlowchartBlock = "Sleep Lose";
        gainStatusBlockName = "Sleep Gain";
        // isPrimary = true;
    }

    public override void Initiate()
    {
        BattleTriggerOnMoveSleepCancel sleepMoveCancelTrigger = new BattleTriggerOnMoveSleepCancel(pokemon, new UseMoveMods(null));
        battleTriggers.Add(sleepMoveCancelTrigger);
        base.Initiate();
    }

    public override void PassTurn()
    {
        if (turnsLeft > 1)
        {
            Flowchart battleFlow = BattleAnimatorMaster.GetInstance().battleFlowchart;
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventNarrative(
                new BattleTriggerMessageData(battleFlow, "Sleep Warning", new Dictionary<string, string>() { { "pokemon", pokemon.GetName() } })
            ));
            StatusEffectData sed = BattleAnimatorMaster.GetInstance().GetStatusEffectData(effectId);
            foreach (BattleAnimation anim in sed.hitAnims)
            {
                BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPokemonMoveAnimation(pokemon, pokemon, anim));
            }
        }
        base.PassTurn();
    }
}

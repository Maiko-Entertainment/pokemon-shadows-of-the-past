using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataOnPokemon : ItemData
{
    public bool equipable;
    public InBattleAutouseCondition autoUseCondition;

    public virtual List<BattleTrigger> InitiateInBattle(PokemonBattleData user)
    {
        List<BattleTrigger> triggers = new List<BattleTrigger>();
        BattleManager battle = BattleMaster.GetInstance().GetCurrentBattle();
        switch (autoUseCondition)
        {
            case InBattleAutouseCondition.HalfHealth:
                BattleTriggerPokemonHalfHealthUseItem trigger = new BattleTriggerPokemonHalfHealthUseItem(user, this, true);
                trigger.maxTriggers = 1;
                triggers.Add(trigger);
                battle.AddTrigger(trigger);
                break;
        }
        return triggers;
    }
    public override ItemTargetType GetItemTargetType()
    {
        return ItemTargetType.Pokemon;
    }
    public virtual CanUseResult CanUseOnPokemon(PokemonCaughtData pokemon)
    {
        return new CanUseResult(equipable, "");
    }

    public virtual CanUseResult CanUseOnPokemonBattle(PokemonBattleData pokemon)
    {
        return new CanUseResult(true, "");
    }

    public virtual void UseOnPokemon(PokemonCaughtData pokemon)
    {
        AudioMaster.GetInstance().PlaySfx(useSound);
        UIPauseMenuMaster.GetInstance().UpdatePartyMiniPreview();
        HandleAfterUse();
    }

    public virtual void UseOnPokemonBattle(PokemonBattleData pokemon, bool isPokemonUsingIt = false)
    {
        BattleTeamId teamId = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(pokemon);
        BattleTeamData battleTeam = BattleMaster.GetInstance().GetCurrentBattle().GetTeamData(teamId);
        BattleAnimatorMaster.GetInstance().AddEventBattleFlowcartTrainerItemText(isPokemonUsingIt ? pokemon.GetName() : battleTeam.GetTrainerTitle(), itemName);
        if (useSound)
        {
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPlaySound(useSound, 1, true));
        }
        PlayAnimations();
        HandleAfterUse();
    }
    public void PlayAnimations(PokemonBattleData pokemon)
    {
        foreach (BattleAnimation anim in animations)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(
                new BattleAnimatorEventPokemonMoveAnimation(pokemon, pokemon, anim)
            );
        }
    }
}

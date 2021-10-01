using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataOnPokemon : ItemData
{
    public override ItemTargetType GetItemTargetType()
    {
        return ItemTargetType.Pokemon;
    }
    public virtual CanUseResult CanUseOnPokemon(PokemonCaughtData pokemon)
    {
        return new CanUseResult(true, "");
    }

    public virtual CanUseResult CanUseOnPokemonBattle(PokemonBattleData pokemon)
    {
        return new CanUseResult(true, "");
    }

    public virtual void UseOnPokemon(PokemonCaughtData pokemon)
    {
        AudioMaster.GetInstance().PlaySfx(useSound);
        HandleAfterUse();
    }

    public virtual void UseOnPokemonBattle(PokemonBattleData pokemon)
    {
        BattleTeamId teamId = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(pokemon);
        BattleTeamData battleTeam = BattleMaster.GetInstance().GetCurrentBattle().GetTeamData(teamId);
        BattleAnimatorMaster.GetInstance().AddEventBattleFlowcartTrainerItemText(battleTeam.GetTrainerTitle(), itemName);
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

using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Items/Pokeball")]
public class ItemDataPokeball : ItemData
{
    public float baseCaptureRate = 1f;
    public List<BattleAnimation> shakeLeftAnim = new List<BattleAnimation>();
    public List<BattleAnimation> shakeRightAnim = new List<BattleAnimation>();
    public List<BattleAnimation> onCatchAnim = new List<BattleAnimation>();
    public List<BattleAnimation> onFailAnim = new List<BattleAnimation>();
    public override CanUseResult CanUse()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (bm!=null && bm.IsBattleActive())
        {
            if (bm.battleData.battleType == BattleType.Pokemon)
            {
                return new CanUseResult(true, "");
            }
            return new CanUseResult(false, "The pokeball won't have any effect.");
        }
        return new CanUseResult(false, "Can only be used in combat");
    }

    public override void Use()
    {
        if (BattleMaster.GetInstance().GetCurrentBattle() != null)
        {
            base.Use();
            BattleMaster.GetInstance().GetCurrentBattle()?
            .HandleTurnInput(new BattleTurnDesitionCapture(
                this,
                BattleTeamId.Team1
            ));
            BattleAnimatorMaster.GetInstance().HideOptions();
        }
    }

    public virtual void UseInBattle()
    {
        PokeballResult result = BattleMaster.GetInstance().GetCurrentBattle().HandlePokeballUse(this);
        PlayAnimations(result);
        if (result.wasCaptured)
        {
            PlaySuccessAnim();
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPlaySound(BattleAnimatorMaster.GetInstance().pokemonCaughtClip,1,true));
            BattleAnimatorMaster.GetInstance().AddEventBattleFlowcartCaptureSuccessText(result.pokemon.GetName());
            BattleMaster.GetInstance()?.ClearBattleEvents();
            BattleMaster.GetInstance()?.GetCurrentBattle()?.HandleBattleEnd(BattleTeamId.Team1, true);
            PartyMaster.GetInstance().AddPartyMember(result.pokemon.GetPokemonCaughtData());
        }
        else
        {
            PlayFailAnim();
            BattleAnimatorMaster.GetInstance().AddEventBattleFlowcartCaptureFailText(result.pokemon.GetName());
        }
    }

    public void PlaySuccessAnim()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        PokemonBattleData pkmn1 = bm.GetTeamActivePokemon(BattleTeamId.Team1);
        PokemonBattleData pkmn2 = bm.GetTeamActivePokemon(BattleTeamId.Team2);
        foreach (BattleAnimation anim in onCatchAnim)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(
                new BattleAnimatorEventPokemonMoveAnimation(pkmn1, pkmn2, anim)
            );
        }
    }

    public void PlayFailAnim()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        PokemonBattleData pkmn1 = bm.GetTeamActivePokemon(BattleTeamId.Team1);
        PokemonBattleData pkmn2 = bm.GetTeamActivePokemon(BattleTeamId.Team2);
        foreach (BattleAnimation anim in onFailAnim)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(
                new BattleAnimatorEventPokemonMoveAnimation(pkmn1, pkmn2, anim)
            );
        }
    }

    public void PlayAnimations(PokeballResult result)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        PokemonBattleData pkmn1 = bm.GetTeamActivePokemon(BattleTeamId.Team1);
        PokemonBattleData pkmn2 = bm.GetTeamActivePokemon(BattleTeamId.Team2);
        foreach (BattleAnimation anim in animations)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(
                new BattleAnimatorEventPokemonMoveAnimation(pkmn1, pkmn2, anim)
            );
        }
        for(int i=0; i < result.shakes; i++)
        {
            List<BattleAnimation> shakeAnims = (i % 2 == 0) ? shakeLeftAnim : shakeRightAnim;
            foreach (BattleAnimation anim in shakeAnims)
            {
                BattleAnimatorMaster.GetInstance()?.AddEvent(
                    new BattleAnimatorEventPokemonMoveAnimation(pkmn1, pkmn2, anim)
                );
            }
        }
    }

    public virtual float GetCaptureRate()
    {
        return baseCaptureRate;
    }

    public override ItemTargetType GetItemTargetType()
    {
        return ItemTargetType.None;
    }
}

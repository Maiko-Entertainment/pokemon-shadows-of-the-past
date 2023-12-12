using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Items/Pokeball")]
public class ItemDataPokeball : ItemData
{
    public float baseCaptureRate = 1f;
    public List<BattleAnimationPokemon> shakeLeftAnim = new List<BattleAnimationPokemon>();
    public List<BattleAnimationPokemon> shakeRightAnim = new List<BattleAnimationPokemon>();
    public List<BattleAnimationPokemon> onCatchAnim = new List<BattleAnimationPokemon>();
    public List<BattleAnimationPokemon> onFailAnim = new List<BattleAnimationPokemon>();
    public override CanUseResult CanUse()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (BattleMaster.GetInstance().IsBattleActive())
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
            PokemonMaster.GetInstance().CaughtPokemon(result.pokemon.GetPokemonCaughtData().pokemonBase.pokemonId);
            bool wasAddedToparty = PartyMaster.GetInstance().AddPartyMember(result.pokemon.GetPokemonCaughtData());
            PlaySuccessAnim();
            BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPlaySound(BattleAnimatorMaster.GetInstance().pokemonCaughtClip,1,true));
            BattleAnimatorMaster.GetInstance().AddEventBattleFlowcartCaptureSuccessText(result.pokemon.GetName());
            if (!wasAddedToparty)
            {
                BattleAnimatorMaster.GetInstance().AddEventBattleFlowcartPokemonBoxSent(result.pokemon.GetName());
            }
            BattleMaster.GetInstance()?.ClearBattleEvents();
            BattleMaster.GetInstance()?.GetCurrentBattle()?.HandleBattleEnd(BattleTeamId.Team1, true);
        }
        else
        {
            PlayFailAnim();
            BattleAnimatorMaster.GetInstance().AddEventBattleFlowcartCaptureFailText(result.pokemon.GetName());
        }
        HandleAfterUse();
    }

    public void PlaySuccessAnim()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        PokemonBattleData pkmn1 = bm.GetTeamActivePokemon(BattleTeamId.Team1);
        PokemonBattleData pkmn2 = bm.GetTeamActivePokemon(BattleTeamId.Team2);
        foreach (BattleAnimationPokemon anim in onCatchAnim)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(
                new BattleAnimatorEventAnimation(pkmn1, pkmn2, anim)
            );
        }
    }

    public void PlayFailAnim()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        PokemonBattleData pkmn1 = bm.GetTeamActivePokemon(BattleTeamId.Team1);
        PokemonBattleData pkmn2 = bm.GetTeamActivePokemon(BattleTeamId.Team2);
        foreach (BattleAnimationPokemon anim in onFailAnim)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(
                new BattleAnimatorEventAnimation(pkmn1, pkmn2, anim)
            );
        }
    }

    public void PlayAnimations(PokeballResult result)
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        PokemonBattleData pkmn1 = bm.GetTeamActivePokemon(BattleTeamId.Team1);
        PokemonBattleData pkmn2 = bm.GetTeamActivePokemon(BattleTeamId.Team2);
        foreach (BattleAnimationPokemon anim in animations)
        {
            BattleAnimatorMaster.GetInstance()?.AddEvent(
                new BattleAnimatorEventAnimation(pkmn1, pkmn2, anim)
            );
        }
        for(int i=0; i < result.shakes; i++)
        {
            List<BattleAnimationPokemon> shakeAnims = (i % 2 == 0) ? shakeLeftAnim : shakeRightAnim;
            foreach (BattleAnimationPokemon anim in shakeAnims)
            {
                BattleAnimatorMaster.GetInstance()?.AddEvent(
                    new BattleAnimatorEventAnimation(pkmn1, pkmn2, anim)
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

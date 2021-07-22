using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Items/Pokeball")]
public class ItemDataPokeball : ItemData
{
    public float baseCaptureRate = 1f;
    public List<BattleAnimation> onCatchAnim = new List<BattleAnimation>();
    public List<BattleAnimation> onFailAnim = new List<BattleAnimation>();
    public override CanUseResult CanUse()
    {
        BattleManager bm = BattleMaster.GetInstance().GetCurrentBattle();
        if (bm!=null)
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
        PlayAnimations();
        if (result.wasCaptured)
        {
            PlaySuccessAnim();
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

    public override void PlayAnimations()
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
    }

    public virtual float GetCaptureRate()
    {
        return baseCaptureRate;
    }
}

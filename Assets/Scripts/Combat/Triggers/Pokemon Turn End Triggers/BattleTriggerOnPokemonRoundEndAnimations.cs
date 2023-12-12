using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerOnPokemonRoundEndAnimations : BattleTriggerOnPokemonRoundEnd
{
    PokemonBattleData target;
    public List<BattleAnimationPokemon> animations;
    public bool userIsOppositeOpposingTeamFromTarget = false;
    public BattleTriggerOnPokemonRoundEndAnimations(PokemonBattleData user, PokemonBattleData target, List<BattleAnimationPokemon> animations):
        base(user)
    {
        this.animations = animations;
        this.target = target;
    }
    public override bool Execute(BattleEventRoundEnd battleEvent)
    {
        if (userIsOppositeOpposingTeamFromTarget)
        {
            BattleTeamId targetsTeam = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(target);
            pokemon = BattleMaster.GetInstance().GetCurrentBattle().GetTeamActivePokemon(targetsTeam == BattleTeamId.Team1 ? BattleTeamId.Team2 : BattleTeamId.Team1);
        }
        if (!target.IsFainted() && !pokemon.IsFainted())
        {
            foreach (BattleAnimationPokemon anim in animations)
            {
                BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventAnimation(pokemon, target, anim));
            }
        }
        return base.Execute(battleEvent);
    }
}

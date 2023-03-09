using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerRoundEndAnimations : BattleTriggerOnPokemonRoundEnd
{
    PokemonBattleData target;
    public List<BattleAnimation> animations;
    public bool userIsOppositeOpposingTeamFromTarget = false;
    public BattleTriggerRoundEndAnimations(PokemonBattleData user, PokemonBattleData target, List<BattleAnimation> animations):
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
            foreach (BattleAnimation anim in animations)
            {
                BattleAnimatorMaster.GetInstance().AddEvent(new BattleAnimatorEventPokemonMoveAnimation(pokemon, target, anim));
            }
        }
        return base.Execute(battleEvent);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves have a list of animations
public class BattleAnimationPokemon : BattleAnimation, IBattleAnimationPokemon
{
    public bool flipXForTeam2 = false;
    public bool flipYForTeam2 = false;

    protected PokemonBattleData user;
    protected PokemonBattleData target;

    // Execute lets the animation know the user, target and that the animation has begun
    public virtual BattleAnimationPokemon Execute(PokemonBattleData user, PokemonBattleData target)
    {
        base.Execute();
        this.user = user;
        this.target = target;
        BattleTeamId userTeam = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(user);
        if (flipXForTeam2 && userTeam == BattleTeamId.Team2)
        {
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
        }
        if (triggerName != "" && animator != null)
            animator.SetTrigger(triggerName);
        return this;
    }

}

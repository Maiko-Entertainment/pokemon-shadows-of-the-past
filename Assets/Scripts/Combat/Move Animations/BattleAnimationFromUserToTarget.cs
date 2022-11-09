using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationFromUserToTarget : BattleAnimation
{
    public Vector2 targetOffset = Vector2.zero;
    protected float timePassed = 0;
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        return base.Execute(user, target);
    }

    private void FixedUpdate()
    {
        BattleTeamId targetTeam = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(target);
        Vector3 origin = BattleAnimatorMaster.GetInstance().GetPokemonPosition(user);
        Vector3 destiny = BattleAnimatorMaster.GetInstance().GetPokemonPosition(target) + (Vector3)(targetTeam == BattleTeamId.Team2 ? targetOffset : -1 * targetOffset);
        transform.position = Vector3.Lerp(origin, destiny, timePassed / destroyAfter);
        timePassed += Time.fixedDeltaTime;
    }
}

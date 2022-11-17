using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationFromUserToTarget : BattleAnimation
{
    public float stayInTargetTime = 0f;
    public Vector2 userOffset = Vector2.zero;
    public Vector2 targetOffset = Vector2.zero;
    protected float timePassed = 0;
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {

        this.user = user;
        this.target = target;
        BattleTeamId userTeam = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(user);
        if (flipXForTeam2 && userTeam == BattleTeamId.Team2)
        {
            transform.localScale.Scale(new Vector3(-1, 1, 1));
        }
        if (flipYForTeam2 && userTeam == BattleTeamId.Team2)
        {
            transform.localScale.Scale(new Vector3(1, -1, 1));
        }
        Destroy(gameObject, destroyAfter + stayInTargetTime);
        return this;
    }

    private void FixedUpdate()
    {
        BattleTeamId targetTeam = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(target);
        Vector3 origin = BattleAnimatorMaster.GetInstance().GetPokemonPosition(user) + (Vector3) userOffset;
        Vector3 destiny = BattleAnimatorMaster.GetInstance().GetPokemonPosition(target) + (Vector3)(targetTeam == BattleTeamId.Team2 ? targetOffset : -1 * targetOffset);
        transform.position = Vector3.Lerp(origin, destiny, timePassed / destroyAfter);
        timePassed += Time.fixedDeltaTime;
    }
}

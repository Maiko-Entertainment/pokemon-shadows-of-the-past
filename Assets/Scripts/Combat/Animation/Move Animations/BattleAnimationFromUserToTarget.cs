using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationFromUserToTarget : BattleAnimationPokemon
{
    public float stayInUserTime = 0f;
    public float stayInTargetTime = 0f;
    public Vector2 userOffset = Vector2.zero;
    public Vector2 targetOffset = Vector2.zero;
    public bool reverse = false;
    protected float timePassed = 0;
    protected Vector3 initialScaling = Vector3.one;
    protected Vector3 finalScaling = Vector3.one;
    public override BattleAnimationPokemon Execute(PokemonBattleData user, PokemonBattleData target)
    {
        this.user = user;
        this.target = target;
        BattleTeamId userTeam = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(user);
        BattleTeamId targetTeam = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(target);

        if (flipXForTeam2 && userTeam == BattleTeamId.Team2)
        {
            transform.localScale.Scale(new Vector3(-1, 1, 1));
        }
        if (flipYForTeam2 && userTeam == BattleTeamId.Team2)
        {
            transform.localScale.Scale(new Vector3(1, -1, 1));
        }
        if (userTeam == BattleTeamId.Team1 && targetTeam == BattleTeamId.Team2)
        {
            finalScaling = transform.localScale * 0.7f;
        }
        else if (userTeam == BattleTeamId.Team2 && targetTeam == BattleTeamId.Team1)
        {
            finalScaling = transform.localScale;
            transform.localScale = transform.localScale * 0.7f;
        }
        else
        {
            finalScaling = transform.localScale;
        }
        initialScaling = transform.localScale;
        Destroy(gameObject, destroyAfter + stayInUserTime + stayInTargetTime);
        return this;
    }

    private void FixedUpdate()
    {
        Vector3 origin = GetOrigin();
        Vector3 destiny = GetTarget();
        float lerpValue = Mathf.Max(timePassed - stayInUserTime, 0) / destroyAfter;
        transform.position = Vector3.Lerp(origin, destiny, lerpValue);
        // Adjust projectile scale to handle perspective
        transform.localScale = Vector3.Lerp(initialScaling, finalScaling, lerpValue);
        timePassed += Time.fixedDeltaTime;
    }

    public Vector3 GetOrigin()
    {
        Vector3 origin = BattleAnimatorMaster.GetInstance().GetPokemonPosition(reverse ? target : user) + (Vector3)userOffset;
        return origin;
    }
    public Vector3 GetTarget()
    {
        PokemonBattleData finalTarget = reverse ? user : target;
        BattleTeamId targetTeam = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(finalTarget);
        Vector3 destiny = BattleAnimatorMaster.GetInstance().GetPokemonPosition(finalTarget) + (Vector3)(targetTeam == BattleTeamId.Team2 ? targetOffset : -1 * targetOffset);
        return destiny;
    }
}

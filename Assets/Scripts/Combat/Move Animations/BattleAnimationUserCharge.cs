using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationUserCharge : BattleAnimation
{
    public Vector2 backMovement;
    public Vector2 frontMovement;

    protected int stage = 0;
    protected Vector3 initialPosition;
    protected Transform teamTransform;
    protected float timePassed = 0;
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        teamTransform = BattleAnimatorMaster.GetInstance().GetPokemonTeamTransform(user);
        initialPosition = teamTransform.localPosition;
        BattleTeamId teamId = BattleMaster.GetInstance().GetCurrentBattle().GetTeamId(user);
        if (teamId == BattleTeamId.Team2)
        {
            backMovement *= -1;
            frontMovement *= -1;
        }
        return base.Execute(user, target);
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        if (stage == 0)
        {
            teamTransform.localPosition = Vector3.Lerp(initialPosition, backMovement, timePassed / destroyAfter / 0.5f);
            if (Vector3.Distance(teamTransform.localPosition, backMovement) == 0)
            {
                stage++;
                timePassed = 0;
            }
        }
        else if (stage == 1)
        {
            teamTransform.localPosition = Vector3.Lerp(backMovement, frontMovement, timePassed / destroyAfter / 0.2f);
            if (Vector3.Distance(teamTransform.localPosition, frontMovement) == 0)
            {
                stage++;
                timePassed = 0;
            }
        }
        else if (stage == 2)
        {
            teamTransform.localPosition = Vector3.Lerp(frontMovement, initialPosition, timePassed / destroyAfter / 0.25f);
            if (Vector3.Distance(teamTransform.localPosition, initialPosition) == 0)
            {
                teamTransform.localPosition = initialPosition;
                stage++;
                timePassed = 0;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationVibrate : BattleAnimation
{
    public Vector2 movement = Vector2.zero;
    public int iterations = 2;
    public bool targetUserInstead = false;
    public bool dontMoveToBothSides = false;

    protected int stage = 0;
    protected int iterationsDone = 0;
    protected Vector2 initialPosition;
    protected Transform teamTransform;
    protected float timePassed = 0;
    protected int direction = 1; 
    public override BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        teamTransform = BattleAnimatorMaster.GetInstance().GetPokemonTeamTransform(targetUserInstead ? user : target);
        initialPosition = teamTransform.localPosition;
        return base.Execute(user, target);
    }

    private void FixedUpdate()
    {
        timePassed += Time.deltaTime;
        if (iterationsDone < iterations)
        {
            if (dontMoveToBothSides)
            {
                if (stage == 0)
                {
                    teamTransform.localPosition = Vector3.Lerp(initialPosition, GetFinalPosition(), timePassed / destroyAfter * iterations * 2f);
                    if (Vector3.Distance(teamTransform.localPosition, GetFinalPosition()) == 0)
                    {
                        stage++;
                        timePassed = 0;
                    }
                }
                else
                {
                    teamTransform.localPosition = Vector3.Lerp(GetFinalPosition(), initialPosition, timePassed / destroyAfter * iterations * 2f);
                    if (Vector3.Distance(teamTransform.localPosition, initialPosition) == 0)
                    {
                        iterationsDone++;
                        stage = 0;
                        timePassed = 0;
                    }
                }
            }
            else
            {
                if (stage == 0)
                {
                    teamTransform.localPosition = Vector3.Lerp(initialPosition, GetFinalPosition(), timePassed / destroyAfter * iterations * 4f);
                    if (Vector3.Distance(teamTransform.localPosition, GetFinalPosition()) == 0)
                    {
                        stage++;
                        direction *= -1;
                        timePassed = 0;
                    }
                }
                else if (stage == 1)
                {
                    teamTransform.localPosition = Vector3.Lerp(initialPosition + movement, GetFinalPosition(), timePassed / destroyAfter * iterations * 2f);
                    if (Vector3.Distance(teamTransform.localPosition, GetFinalPosition()) == 0)
                    {
                        stage++;
                        direction *= -1;
                        timePassed = 0;
                    }
                }
                else
                {
                    teamTransform.localPosition = Vector3.Lerp(initialPosition + movement * -1f, initialPosition, timePassed / destroyAfter * iterations * 4f);
                    if (Vector3.Distance(teamTransform.localPosition, initialPosition) == 0)
                    {
                        iterationsDone++;
                        stage = 0;
                        timePassed = 0;
                    }
                }
            }

        }
    }

    public Vector2 GetFinalPosition()
    {
        return initialPosition + movement * direction;
    }

    private void OnDestroy()
    {
        teamTransform.localPosition = initialPosition;
    }
}

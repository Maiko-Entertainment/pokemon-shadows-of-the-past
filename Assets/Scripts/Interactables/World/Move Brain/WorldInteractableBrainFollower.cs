using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractableBrainFollower : WorldInteractableMoveBrain
{
    public bool followMode = true;


    protected override void Update()
    {
        animator.GetComponentInChildren<SpriteRenderer>().sortingOrder = (int)transform.position.y * -1;
        if (followMode)
        {
            if (HasReachedTarget())
            {
                if (!animator.GetBool("Moving"))
                    animator.SetBool("Moving", false);
            }
            else
            {
                Vector3 direction = ((Vector3)target - transform.position).normalized;
                if (animator.GetBool("Moving")) animator.SetBool("Moving", true);
                animator.SetFloat("Horizontal", direction.x);
                animator.SetFloat("Vertical", direction.y);
                float playerSpeed = WorldMapMaster.GetInstance().GetPlayer().speed;
                transform.position = Vector2.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);
            }
        }
        else
        {
            base.Update();
        }
    }
}

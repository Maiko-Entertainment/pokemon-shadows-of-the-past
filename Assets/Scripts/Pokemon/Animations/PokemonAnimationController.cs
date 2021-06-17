using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonAnimationController : MonoBehaviour
{
    public Animator animator;

    public void TriggerIdle()
    {
        animator.SetTrigger("Idle");
    }
    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }
    public void TriggerSpecial()
    {
        animator.SetTrigger("Special");
    }
    public void TriggerBack()
    {
        animator.SetTrigger("Back");
    }

    public void Trigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }
}

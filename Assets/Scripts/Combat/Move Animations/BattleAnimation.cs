using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves have a list of animations
public class BattleAnimation : MonoBehaviour
{
    public Animator animator;
    public float duration; // Lets animator manager know when to go to next anim
    public float destroyAfter;
    public string triggerName = "";

    protected PokemonBattleData user;
    protected PokemonBattleData target;

    // Execute lets the animation know the user, target and that the animation has begun
    public virtual BattleAnimation Execute(PokemonBattleData user, PokemonBattleData target)
    {
        this.user = user;
        this.target = target;
        if (triggerName != "" && animator != null)
            animator.SetTrigger(triggerName);
        Destroy(gameObject, destroyAfter);
        return this;
    }

}

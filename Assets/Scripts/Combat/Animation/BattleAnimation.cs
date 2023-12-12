using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimation : MonoBehaviour, IBattleAnimation
{
    public Animator animator;
    public float duration; // Lets animator manager know when to go to next anim
    public float destroyAfter;
    public string triggerName = "";

    public virtual BattleAnimation Execute()
    {
        Destroy(gameObject, destroyAfter);
        return this;
    }
}

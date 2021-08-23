using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonAnimationController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer sprite;

    bool isBack = false;
    protected SpriteRenderer shadow;

    private void Start()
    {
        AddShadow();
    }

    public void TriggerIdle()
    {
        sprite.sortingLayerName = "Pokemon Enemy";
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
        sprite.sortingLayerName = "Pokemon Player";
        animator.SetTrigger("Back");
        isBack = true;
        AddShadow();
    }

    public void Trigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void AddShadow()
    {
        if (shadow == null)
        {
            Material shadowMaterial = BattleAnimatorMaster.GetInstance().shadowMaterial;
            shadow = Instantiate(sprite, sprite.transform);
            shadow.transform.localPosition = new Vector3(0, 0.05f, 0);
            shadow.transform.localScale = new Vector3(1, 1, 1);
            if (isBack)
            {
                shadow.GetComponent<Animator>().SetTrigger("Back");
            }
            shadow.transform.eulerAngles = new Vector3(50, 0, 20);
            shadow.GetComponent<Renderer>().material = shadowMaterial;
        }
    }

    public void SetPokemonSpriteMaterial(Material material)
    {
        sprite.GetComponent<Renderer>().material = material;
    }
}

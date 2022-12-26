using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo(
    "Scripting",
    "Destory Children by Index",
    "Destroys the children object of a parent component using the index provided."
)]
public class FungusDestroyChildIndex : Command
{
    public Transform parent;
    public int destroyIndex = 0;

    public override void OnEnter()
    {
        if (parent.childCount > destroyIndex)
        {
            Transform child = parent.GetChild(destroyIndex);
            if (child) Destroy(child.gameObject);
        }
        Continue();
    }
}

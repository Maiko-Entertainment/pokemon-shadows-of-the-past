using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInteractableGetItem : WorldInteractableTouch
{
    public ItemData item;
    public int minAmountItems = 1;
    public int extraRange = 0;
    public bool destroyOnInteract = true;
    public override void OnInteract()
    {
        int random = Random.Range(minAmountItems, minAmountItems + extraRange + 1);
        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "item", item.GetName() },
            { "itemId", item.GetItemId() },
            { "amountString", "" + random }
        };
        InteractionsMaster.GetInstance()?.AddEvent(
            new InteractionEventFlowchart(
                flowchartPrefab,
                blockName,
                values
        ));
        if (destroyOnInteract)
        {
            InteractionsMaster.GetInstance().AddEvent(new InteractionEventDestroyInstance(new List<GameObject>() { gameObject }));
        }
    }
}

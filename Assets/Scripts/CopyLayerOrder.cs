using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLayerOrder : MonoBehaviour
{
    public SpriteRenderer rendererToCopy;
    public int difference = 0;
    protected SpriteRenderer spriteRender;
    // Start is called before the first frame update
    void Start()
    {
        spriteRender = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (rendererToCopy)
        {
            spriteRender.sortingLayerName = rendererToCopy.sortingLayerName;
            spriteRender.sortingOrder = difference + rendererToCopy.sortingOrder;
        }
    }
}

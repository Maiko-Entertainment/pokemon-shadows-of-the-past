using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidObject : MonoBehaviour
{
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    public int offset = 0;
    void Update()
    {
        foreach(SpriteRenderer sp in sprites)
        {
            sp.sortingOrder = (int)Mathf.Ceil(transform.position.y) * -1 + offset;
        }
    }
}

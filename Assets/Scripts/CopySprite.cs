using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopySprite : MonoBehaviour
{
    public SpriteRenderer target;
    public SpriteRenderer copyTo;

    // Update is called once per frame
    void Update()
    {
        copyTo.sprite = target.sprite;
    }
}

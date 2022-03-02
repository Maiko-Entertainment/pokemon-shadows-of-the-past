using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SerializableVector2
{
    public float x;
    public float y;

    public SerializableVector2(Vector2 vector)
    {
        x = vector.x;
        y = vector.y;
    }

    public Vector2 GetVector2()
    {
        return new Vector2(x, y);
    }
}

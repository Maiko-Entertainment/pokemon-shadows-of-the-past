using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorEventDestroy : BattleAnimatorEvent
{
    List<GameObject> _gameObjects = new List<GameObject>();
    public BattleAnimatorEventDestroy(List<GameObject> objects) : base()
    {
        dontWait = true;
        _gameObjects = objects;
    }

    public BattleAnimatorEventDestroy(GameObject objectToDestroy) : base()
    {
        dontWait = true;
        _gameObjects = new List<GameObject>(){ objectToDestroy };
    }

    public override void Execute()
    {
        base.Execute();
        foreach (GameObject obj in _gameObjects)
        {
            if (obj != null)
            {
                Object.Destroy(obj);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncActivation : MonoBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();

    private void OnEnable()
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(true);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(false);
        }
    }
}

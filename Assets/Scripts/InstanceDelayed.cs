using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceDelayed : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale = Vector3.one;
    public bool isChild;
    public float delay = 0.5f;


    private void Start()
    {
        StartCoroutine(Spawn(delay));
    }

    IEnumerator Spawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject instance = Instantiate(prefab, isChild ? transform : null);
        instance.transform.localPosition = position;
        instance.transform.localScale = scale;
        instance.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }
}

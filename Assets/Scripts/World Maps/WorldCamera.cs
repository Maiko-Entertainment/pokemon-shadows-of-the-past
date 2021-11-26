using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCamera : MonoBehaviour
{
    public Vector3 offset = Vector3.zero;
    public float offsetAdjustSpeed = 2;
    private Transform player;
    private Vector3 currentOffset = Vector3.zero;
    void Start()
    {
        LookForPlayer();
    }

    public void LookForPlayer()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go)
        {
            player = go.transform;

        }
    }

    public float SetOffset(Vector3 newOffset)
    {
        float distance = Vector3.Distance(offset, newOffset);
        offset = newOffset;
        return distance / offsetAdjustSpeed;
    }

    public void ResetOffset()
    {
        offset = Vector3.zero;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player)
        {
            currentOffset = Vector3.MoveTowards(currentOffset, offset, offsetAdjustSpeed * Time.deltaTime);
            transform.position = player.position + currentOffset + Vector3.forward * -1;
        }
        else
        {
            LookForPlayer();
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCamera : MonoBehaviour
{
    Transform player;
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

    // Update is called once per frame
    void LateUpdate()
    {
        if (player)
        {
            transform.position = player.position + Vector3.forward * -1;
        }
        else
        {
            LookForPlayer();
        }
    }


}

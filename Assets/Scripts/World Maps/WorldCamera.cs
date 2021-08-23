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
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.position + Vector3.forward * -1;
    }


}

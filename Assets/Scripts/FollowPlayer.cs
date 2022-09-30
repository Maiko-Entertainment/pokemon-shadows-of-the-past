using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float speed = 10;
    void Update()
    {
        PlayerController player = WorldMapMaster.GetInstance().GetPlayer();
        if (player)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
        }
    }
}

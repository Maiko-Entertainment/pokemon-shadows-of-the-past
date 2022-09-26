using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapDamageTriggerer : MonoBehaviour
{
    public WorldMapDamageDealerEnterArea damageDealer;

    public float activationMinInterval = 2f;
    protected float timeLeft = 0;

    private void Start()
    {
        timeLeft = activationMinInterval;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && timeLeft < 0)
        {
            damageDealer.TriggerEnterArea();
            timeLeft = activationMinInterval;
        }
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
    }
}

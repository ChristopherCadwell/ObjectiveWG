using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBlock : MonoBehaviour
{
    [Header("How much damage to apply when player contacts this block")]public float damage = 10;
    [Header("Respawn at last grounded position")] public bool useGroundedRespawn;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health health = collision.GetComponent<Health>();
            health.Damage(damage);
            Debug.Log("Current health = " + health.CurrentHealth);
            if (health.CurrentHealth > 0)
            {
                if (useGroundedRespawn)
                {
                    GameManager.Instance.HoleReSpawn();
                }
                else
                    collision.transform.position = GameManager.Instance.lastCheckPoint.transform.position;
            }
            
        }
    }
}

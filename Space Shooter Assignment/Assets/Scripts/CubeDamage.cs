using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CubeDamage : MonoBehaviour
{
    public int damagePerHit = 10;
    public float damageInterval = 1f;       // seconds between damage ticks

    private float damageTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (damageTimer > 0f)
            damageTimer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (damageTimer > 0f) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damagePerHit);
                damageTimer = damageInterval;
            }
        }
    }
}

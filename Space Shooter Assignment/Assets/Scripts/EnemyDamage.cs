using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damagePerHit = 10;
    public float damageInterval = 1f;   // damage every X seconds while inside

    public AlienBeetle beetleAI;        // drag the beetle root here in Inspector

    private float damageTimer = 0f;

    private void Update()
    {
        if (damageTimer > 0f)
            damageTimer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (damageTimer <= 0f)
        {
            // Deal damage
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damagePerHit);
                damageTimer = damageInterval;
            }

            // Ask the beetle to play an attack animation (with its own cooldown)
            if (beetleAI != null)
            {
                beetleAI.TryPlayAttack();
            }
        }
    }
}

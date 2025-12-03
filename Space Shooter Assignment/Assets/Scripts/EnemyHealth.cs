using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;
    private bool isDead = false;

    private AlienBeetle beetleAI;

    private void Awake()
    {
        currentHealth = maxHealth;
        beetleAI = GetComponent<AlienBeetle>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("EnemyHealth: took " + amount + " damage, now " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("EnemyHealth.Die() called");

        if (beetleAI != null)
        {
            beetleAI.Die();
        }
    }
}

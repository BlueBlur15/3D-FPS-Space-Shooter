using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    public int damage = 10;

    void Start()
    {
        // Destroy this bullet after lifeTime seconds
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Move straight forward every frame
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1) Try to find an EnemyHealth on what we hit (or its parent)
        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // 2) Don't hurt the player with their own bullets
        if (other.GetComponent<PlayerHealth>() != null)
        {
            return;
        }

        // 3) If we hit any solid, non-trigger thing, destroy bullet
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;

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
}

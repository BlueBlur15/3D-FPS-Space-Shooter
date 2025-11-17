using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    public string gunName = "Pistol";
    public FireType fireType = FireType.Semi;  // Semi or Auto

    // Seconds between shots (e.g. 0.25 = 4 shots per second
    public float fireCooldown = 0.25f;

    // Bullet settings (these get copied into SimpleBullet)
    public float bulletSpeed = 20f;
    public float bulletLifetime = 2f;

    [Header("References")]
    public Transform firePoint;                 // Where bullets spawn
    public GameObject bulletPrefab;             // The projectile

    private float nextFireTime = 0f;

    public void TryShoot()
    {
        // Semi-auto: click once per shot
        if (fireType == FireType.Semi)
        {
            if (Input.GetButtonDown("Fire1"))
                ShootIfReady();
        }
        // Full-auto: hold to spray
        else if (fireType == FireType.Auto)
        {
            if (Input.GetButton("Fire1"))
                ShootIfReady();
        }
    }

    void ShootIfReady()
    {
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireCooldown;
        Shoot();
    }

    public void Shoot()
    {
        if (firePoint == null || bulletPrefab == null)
        {
            Debug.LogWarning($"{gunName}: Missing firePoint or bulletPrefab!");
            return;
        }

        // Spawn bullet
        GameObject b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Set bullet speed & lifetime, if it has SimpleBullet on it
        SimpleBullet sb = b.GetComponent<SimpleBullet>();
        if (sb!= null)
        {
            sb.speed = bulletSpeed;
            sb.lifeTime = bulletLifetime;
        }

        // Play fire sound
        AudioManager.instance.PlaySFX(AudioManager.instance.pistolFireSound);
    }
}

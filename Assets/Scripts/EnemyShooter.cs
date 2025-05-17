using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public Transform player;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    public float bulletSpeed = 10f;
    public float shootRadius = 5f;
    public LayerMask playerLayer;

    private float fireCooldown;

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootRadius)
        {
            fireCooldown -= Time.deltaTime;

            if (fireCooldown <= 0f)
            {
                fireCooldown = 1f / fireRate;
                ShootAtPlayer();
            }

            // Вращаемся к игроку
            Vector3 lookDir = (player.position - transform.position).normalized;
            lookDir.y = 0f;
            if (lookDir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }

    void ShootAtPlayer()
    {
        Vector3 dir = (player.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = dir * bulletSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, shootRadius);
    }
}

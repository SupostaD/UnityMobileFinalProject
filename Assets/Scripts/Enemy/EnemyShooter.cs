using UnityEngine;
public class EnemyShooter : MonoBehaviour
{
    public Transform Player;
    public Transform FirePoint;
    public GameObject BulletPrefab;
    public float FireRate = 1f;
    public float BulletSpeed = 10f;
    public float ShootRadius = 5f;
    public LayerMask PlayerLayer;
    public Collider shooterCollider;

    private float fireCooldown;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (distanceToPlayer <= ShootRadius)
        {
            fireCooldown -= Time.deltaTime;

            if (fireCooldown <= 0f)
            {
                fireCooldown = 1f / FireRate;
                ShootAtPlayer();
            }
        }
    }

    void ShootAtPlayer()
    {
        Vector3 dir = (Player.position - FirePoint.position).normalized;


        Vector3 lookDir = Player.position - transform.position;
        lookDir.y = 0f;

        if (lookDir.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        RaycastHit hit;
        float distance = Vector3.Distance(FirePoint.position, Player.position);

        if (Physics.Raycast(FirePoint.position, dir, out hit, distance))
        {
            if (hit.transform != Player.transform)
            {
                return;
            }
        }

        GameObject bullet = EnemyBulletPool.Instance.GetBullet(FirePoint.position, Quaternion.LookRotation(dir));
        
        Collider bulletCollider = bullet.GetComponent<Collider>();
        
        if (bulletCollider != null && shooterCollider != null)
        {
            Physics.IgnoreCollision(bulletCollider, shooterCollider);
        }
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = dir * BulletSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, ShootRadius);
    }
}

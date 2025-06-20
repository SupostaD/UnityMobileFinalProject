using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject BulletPrefab;
    public float BulletSpeed = 10f;
    public LayerMask EnemyLayer;
    public float FireRate = 1f;
    public CircleDrawerDynamicAim AimCircle;
    public PlayerRoll PlayerRoll;
    public float MinShootRadius = 5.1f;
    public Collider shooterCollider;
    public Animator animator;

    private float fireCooldown;

    void Update()
    {
        if (PlayerRoll.IsRolling())
            return;

        fireCooldown -= Time.deltaTime;

        float radius = AimCircle != null ? AimCircle.currentRadius : 5f;

        if (radius <= MinShootRadius)
            return;

        Collider[] enemies = Physics.OverlapSphere(transform.position, radius * 2, EnemyLayer);

        if (enemies.Length > 0 && fireCooldown <= 0f)
        {
            Transform target = FindClosestEnemy(enemies);
            if (target != null)
            {
                Shoot(target);
                fireCooldown = 1f / FireRate;
            }
        }
    }

    Transform FindClosestEnemy(Collider[] enemies)
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var col in enemies)
        {
            float dist = (col.transform.position - transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closest = col.transform;
            }
        }
        return closest;
    }

    void Shoot(Transform target)
    {
        Vector3 dir = (target.position - FirePoint.position).normalized;

        Vector3 lookDir = target.position - transform.position;
        lookDir.y = 0f;

        if (lookDir.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        RaycastHit hit;
        float distance = Vector3.Distance(FirePoint.position, target.position);

        if (Physics.Raycast(FirePoint.position, dir, out hit, distance))
        {
            if (hit.transform != target)
            {
                return;
            }
        }

        GameObject bullet = PlayerBulletPool.Instance.GetBullet(FirePoint.position, Quaternion.LookRotation(dir));
        
        Collider bulletCollider = bullet.GetComponent<Collider>();
        
        if (bulletCollider != null && shooterCollider != null)
        {
            Physics.IgnoreCollision(bulletCollider, shooterCollider);
        }
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        
        animator.SetBool("isShooting", true);
        
        rb.linearVelocity = dir * BulletSpeed;
    }
}

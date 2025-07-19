using JetBrains.Annotations;
using UnityEngine;
public class EnemyShooter : MonoBehaviour
{
    public Transform FirePoint;
    public float FireRate = 1f;
    public float BulletSpeed = 10f;
    public float ShootRadius = 5f;
    public LayerMask PlayerLayer;
    public Collider shooterCollider;
    public Animator animator;

    private float fireCooldown;

    void Update()
    {
        Collider[] playersInRange = Physics.OverlapSphere(transform.position, ShootRadius, PlayerLayer);

        if (playersInRange.Length == 0)
        {
            animator.SetBool("isShooting", false);
            return;
        }

        Transform? closestPlayer = FindClosestTarget(playersInRange);

        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f && closestPlayer != null)
        {
            fireCooldown = 1f / FireRate;
            ShootAtTarget(closestPlayer);
        }
    }

    void ShootAtTarget(Transform target)
    {
        Vector3 dir = (target.position - FirePoint.position).normalized;

        Vector3 lookDir = target.position - transform.position;
        lookDir.y = 0f;

        if (lookDir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(lookDir);

        float distance = Vector3.Distance(FirePoint.position, target.position);

        if (Physics.Raycast(FirePoint.position, dir, out RaycastHit hit, distance))
        {
            if (hit.transform != target)
                return;
        }

        GameObject bullet = EnemyBulletPool.Instance.GetBullet(FirePoint.position, Quaternion.LookRotation(dir));

        Collider bulletCollider = bullet.GetComponent<Collider>();
        if (bulletCollider != null && shooterCollider != null)
            Physics.IgnoreCollision(bulletCollider, shooterCollider);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            animator.SetBool("isShooting", true);
            rb.linearVelocity = dir * BulletSpeed;
        }
    }

    [CanBeNull]
    Transform FindClosestTarget(Collider[] targets)
    {
        Transform closest = null;
        float minSqrDist = Mathf.Infinity;

        foreach (var col in targets)
        {
            float sqrDist = (col.transform.position - transform.position).sqrMagnitude;
            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                closest = col.transform;
            }
        }

        return closest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, ShootRadius);
    }
}

using UnityEngine;
public class EnemyShooter : MonoBehaviour
{
    public Transform firePoint;
    public float shootCooldown = 2f;
    public float bulletSpeed = 10f;
    public Collider shooterCollider;
    public Animator animator;

    private float cooldownTimer = 0f;
    private Transform target;

    void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            animator?.SetBool("isShooting", false);
            return;
        }

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            Shoot();
            cooldownTimer = shootCooldown;
        }
    }

    private void Shoot()
    {
        if (firePoint == null || target == null) return;

        Vector3 direction = (target.position - firePoint.position).normalized;

        Vector3 lookDir = target.position - transform.position;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(lookDir);

        float distance = Vector3.Distance(firePoint.position, target.position);
        int mask = ~LayerMask.GetMask("IgnoreRaycast");
        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, distance, mask))
        {
            Debug.DrawRay(firePoint.position, direction * distance, Color.red, 0.5f);
            if (!hit.transform.CompareTag("Player"))
            {
                Debug.Log("Blocked shot by: " + hit.transform.name);
                return;
            }
        }

        GameObject bullet = EnemyBulletPool.Instance.GetBullet(firePoint.position, Quaternion.LookRotation(direction));
        if (bullet == null)
        {
            Debug.LogWarning("Bullet pool returned null!");
            return;
        }

        Collider bulletCol = bullet.GetComponent<Collider>();
        if (bulletCol != null && shooterCollider != null)
            Physics.IgnoreCollision(bulletCol, shooterCollider);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        AudioManager.Instance?.PlaySFX("Shoot");
        //animator?.SetBool("isShooting", true);

        Debug.Log("Enemy shot at: " + target.name);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        animator.SetBool("isShooting", true);
    }

    public void ClearTarget()
    {
        target = null;
        animator.SetBool("isShooting", false);
    }
}

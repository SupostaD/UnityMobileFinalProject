using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject BulletPrefab;
    public float BulletSpeed = 10f;
    public LayerMask EnemyLayer;
    public float FireRate = 1f;
    public CircleDrawerDynamicAim AimCircle;

    private float fireCooldown;

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        float radius = AimCircle != null ? AimCircle.currentRadius : 5f;

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
        GameObject bullet = Instantiate(BulletPrefab, FirePoint.position, Quaternion.LookRotation(dir));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = dir * BulletSpeed;
        }
    }
}

using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float Delay = 2f;
    public float ExplosionRadius = 3f;
    public int Damage = 2;
    public LayerMask EnemyLayer;

    private bool fuseStarted = false;

    public void StartFuse()
    {
        if (fuseStarted) return;
        fuseStarted = true;
        Invoke(nameof(Explode), Delay);
    }

    void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, ExplosionRadius, EnemyLayer);

        foreach (var hit in hits)
        {
            Health health = hit.GetComponent<Health>();
            health.TakeDamage(Damage);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }
}

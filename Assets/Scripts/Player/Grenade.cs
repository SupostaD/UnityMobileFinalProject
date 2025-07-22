using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float Delay = 2f;
    public float ExplosionRadius = 3f;
    public int Damage = 2;
    public LayerMask EnemyLayer;

    private bool fuseStarted = false;
    private float explodeTime;

    public void StartFuse()
    {
        if (fuseStarted) return;
        fuseStarted = true;
        explodeTime = Time.time + Delay;
        Invoke(nameof(Explode), Delay);
    }
    
    public GrenadeSaveData GetSaveData()
    {
        float remaining = Mathf.Max(explodeTime - Time.time, 0f);
        return new GrenadeSaveData
        {
            position = transform.position,
            velocity = GetComponent<Rigidbody>().linearVelocity,
            remainingFuseTime = remaining
        };
    }
    
    public void ApplySaveData(GrenadeSaveData data)
    {
        transform.position = data.position;
        GetComponent<Rigidbody>().linearVelocity = data.velocity;
        fuseStarted = true;
        explodeTime = Time.time + data.remainingFuseTime;
        Invoke(nameof(Explode), data.remainingFuseTime);
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

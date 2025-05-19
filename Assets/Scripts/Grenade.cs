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
        Collider[] enemies = Physics.OverlapSphere(transform.position, ExplosionRadius, EnemyLayer);
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }
}

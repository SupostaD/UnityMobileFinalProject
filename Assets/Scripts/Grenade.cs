using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 2f;
    public float explosionRadius = 3f;
    public int damage = 2;
    public LayerMask enemyLayer;

    private bool fuseStarted = false;

    public void StartFuse()
    {
        if (fuseStarted) return;
        fuseStarted = true;
        Invoke(nameof(Explode), delay);
    }

    void Explode()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

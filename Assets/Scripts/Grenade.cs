using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 2f;
    public float explosionRadius = 3f;
    public int damage = 2;
    public LayerMask enemyLayer;
    public GameObject explosionEffect;

    private void Start()
    {
        Invoke("Explode", delay);
    }

    void Explode()
    {
        if (explosionEffect)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        foreach (var enemy in enemies)
        {
            Debug.Log("Enemy hit by grenade: " + enemy.name);
            // enemy.GetComponent<EnemyHealth>().TakeDamage(damage); // если есть
            Destroy(enemy.gameObject); // пока просто уничтожим
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

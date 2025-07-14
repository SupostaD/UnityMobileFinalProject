using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnRadius = 5f;
    public float spawnInterval = 3f;
    public LayerMask enemyLayer;
    public LayerMask playerLayer;

    private float spawnTimer;

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            TrySpawnEnemies();
            spawnTimer = spawnInterval;
        }
    }

    void TrySpawnEnemies()
    {
        foreach (var point in spawnPoints)
        {
            if (point == null) continue;

            bool hasEnemyNearby = Physics.CheckSphere(point.position, spawnRadius, enemyLayer);
            bool hasPlayerNearby = Physics.CheckSphere(point.position, spawnRadius, playerLayer);

            if (!hasEnemyNearby && !hasPlayerNearby)
            {
                GameObject enemyGO = Instantiate(enemyPrefab, point.position, Quaternion.identity);

                Enemy enemy = enemyGO.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.SetId(System.Guid.NewGuid().ToString());
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (spawnPoints == null) return;

        Gizmos.color = Color.red;
        foreach (var point in spawnPoints)
        {
            if (point != null)
                Gizmos.DrawWireSphere(point.position, spawnRadius);
        }
    }
}

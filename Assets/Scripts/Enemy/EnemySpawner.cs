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
    public bool suppressSpawning = false;


    private float spawnTimer;
    private bool enemiesLoadedFromSave = false;

    public void DisableSpawningFromSave()
    {
        enemiesLoadedFromSave = true;
        suppressSpawning = true;
    }

    void Update()
    {
        if (suppressSpawning || enemiesLoadedFromSave) return;
    
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
                Instantiate(enemyPrefab, point.position, Quaternion.identity);
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

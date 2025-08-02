using System.Collections;
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
    public bool waitForLoad = true;

    private float spawnTimer;

    private void Start()
    {
        StartCoroutine(WaitThenSpawn());
    }
    
    private IEnumerator WaitThenSpawn()
    {
        // Ждём пока SaveApplier разрешит
        yield return new WaitUntil(() => waitForLoad == false);

        SpawnEnemies();
    }
    
    void Update()
    {
        if (!waitForLoad)
            SpawnEnemies();
    }
    
    public void SpawnEnemies()
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

using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnRadius = 5f;
    public float spawnInterval = 20f;
    public LayerMask enemyLayer;
    public LayerMask playerLayer;

    private float spawnTimer;
    private bool spawnAllowed = false;

    private void Start()
    {
        if (GameManager.Instance.IsLoadingFromSave)
        {
            StartCoroutine(WaitForSaveApply());
        }
        else
        {
            spawnAllowed = true;
        }
    }

    private IEnumerator WaitForSaveApply()
    {
        yield return new WaitUntil(() => GameManager.Instance.IsLoadingFromSave == false);
        spawnAllowed = true;
    }

    void Update()
    {
        if (!spawnAllowed) return;
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
        int currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        int maxEnemies = 10;

        if (currentEnemies >= maxEnemies)
            return;

        foreach (var point in spawnPoints)
        {
            if (point == null) continue;

            bool hasEnemyNearby = Physics.CheckSphere(point.position, spawnRadius, enemyLayer);
            bool hasPlayerNearby = Physics.CheckSphere(point.position, spawnRadius, playerLayer);

            if (!hasEnemyNearby && !hasPlayerNearby)
            {
                GameObject enemyGO = Instantiate(enemyPrefab, point.position, Quaternion.identity);

                EnemyAI ai = enemyGO.GetComponent<EnemyAI>();
                if (ai != null)
                    ai.SetPatrolPoints(spawnPoints);

                currentEnemies++;

                if (currentEnemies >= maxEnemies)
                    break;
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

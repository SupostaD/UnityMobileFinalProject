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

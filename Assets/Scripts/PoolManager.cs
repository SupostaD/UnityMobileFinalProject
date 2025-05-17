using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int poolSize = 10;
    public Transform[] spawnPoints;

    private GameObject[] enemyPool;

    void Start()
    {
        CreateEnemyPool();
    }

    void CreateEnemyPool()
    {
        enemyPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = spawnPoints[i % spawnPoints.Length].position;
            
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.SetInactiveState();
            
            enemyPool[i] = enemy;
        }
    }
}

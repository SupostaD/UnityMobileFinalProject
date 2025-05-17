using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public EnemyPoolManager enemyPool;

    public void SpawnEnemies()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            var type = enemyPool.enemyTypes[Random.Range(0, enemyPool.enemyTypes.Count)];
            var enemy = enemyPool.GetEnemy(type);
            if (enemy != null)
            {
                enemy.transform.position = spawnPoint.position;
                enemy.transform.rotation = spawnPoint.rotation;
            }
        }
    }
}

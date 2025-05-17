using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public List<EnemyType> enemyTypes;
    public int poolSize = 10;

    private Dictionary<EnemyType, Queue<Enemy>> pools = new();

    private void Awake()
    {
        foreach (var type in enemyTypes)
        {
            var queue = new Queue<Enemy>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(type.prefab, transform);
                obj.SetActive(false);
                Enemy enemy = obj.GetComponent<Enemy>();
                enemy.enemyType = type;
                enemy.OnDeath = ReturnToPool;
                queue.Enqueue(enemy);
            }

            pools[type] = queue;
        }
    }

    public Enemy GetEnemy(EnemyType type)
    {
        if (pools[type].Count > 0)
        {
            var enemy = pools[type].Dequeue();
            enemy.gameObject.SetActive(true);
            return enemy;
        }
        else
        {
            return null;
        }
    }

    public void ReturnToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        pools[enemy.enemyType].Enqueue(enemy);
    }
}

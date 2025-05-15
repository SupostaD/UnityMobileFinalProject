using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    public GameObject enemy;
    public int amountOfEnemies;

    private List<GameObject> enemies;
    
    void Start()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        enemies = new List<GameObject>();
        
        for (int i = 0; i < amountOfEnemies; i++)
        {
            GameObject newEnemy = Instantiate(enemy);
            ReturnToPool(newEnemy);
            enemies.Add(newEnemy);
        }
    }

    public GameObject GetPoolObject()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].activeInHierarchy)
                return enemies[i];
        }
        return null;
    }
    
    void ReturnToPool(GameObject poolObject)
    {
        poolObject.SetActive(false);
    }
}

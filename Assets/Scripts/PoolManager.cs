using UnityEngine;

public class PoolManager : MonoBehaviour
{

    public GameObject enemy;
    public int amountOfEnemies = 10;
    public Transform[] spawnPoints;

    private GameObject[] enemies;
    
    void Start()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        enemies = new GameObject[amountOfEnemies];
        
        for (int i = 0; i < amountOfEnemies; i++)
        {
            GameObject newEnemy = Instantiate(enemy);
            newEnemy.transform.position = spawnPoints[i].position;
            newEnemy.GetComponent<EnemyManager>().Initialize();
            enemies[i] = newEnemy;
        }
    }

    /*public GameObject GetPoolObject()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].activeInHierarchy)
                return enemies[i];
        }
        return null;
    }
    
    public void ReturnToPool(GameObject poolObject)
    {
        poolObject.SetActive(false);
    }*/
}

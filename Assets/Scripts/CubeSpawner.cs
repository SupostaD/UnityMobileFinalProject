using UnityEngine;
using UnityEngine.Rendering;

public class CubeSpawner : MonoBehaviour
{

    public static GameObject prefab;

    // ObjectPool<GameObject> pool = 
    //     new ObjectPool<GameObject>(CreatePooledItem(), OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject(), false, 10, maxPoolSize);
    // CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, 10, maxPoolSize

    private static void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }
    private static void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
    
    private static void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }
    private static GameObject CreatePooledItem()
    {
        GameObject newGameObjectInThePool = Instantiate(prefab);
        return newGameObjectInThePool;
    }

    private GameObject newObject;
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            // newObject = pool.Get();
        }

        if (Input.GetKey(KeyCode.B))
        {
            // pool.Release(newObject);
        }
    }
}

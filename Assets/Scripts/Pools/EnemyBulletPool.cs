using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 40;

    private List<GameObject> bullets = new();

    public static EnemyBulletPool Instance;

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullets.Add(bullet);
        }
    }

    public GameObject GetBullet(Vector3 position, Quaternion rotation)
    {
        foreach (GameObject bullet in bullets)
        {
            if (bullet != null && !bullet.activeInHierarchy)
            {
                bullet.transform.SetPositionAndRotation(position, rotation);
                bullet.SetActive(true);
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab, position, rotation);
        newBullet.SetActive(true);
        bullets.Add(newBullet);
        return newBullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        
        bullet.SetActive(false);
    }
}
